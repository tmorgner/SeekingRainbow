using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace SeekingRainbow.Scripts
{
  /// <summary>
  ///  Attach this to any object that can move somehow.
  /// </summary>
  [RequireComponent(typeof(BoxCollider2D))]
  [RequireComponent(typeof(Rigidbody2D))]
  public class MoveableBehaviour : MonoBehaviour
  {
    public float moveTime = 0.1f; //Time it will take object to move, in seconds.
    public LayerMask blockingLayer; //Layer on which collision will be checked.
    public LayerMask forceWalkableLayer; //Layer on which collision will be checked.
    public UnityEvent OnCantMove;
    public UnityEvent OnMoveStarted;
    public UnityEvent OnMoveFinished;

    public bool Moving { get; set; }
    BoxCollider2D boxCollider; 
    Rigidbody2D rb2D; 
    float inverseMoveTime; 

    protected void Start()
    {
      boxCollider = GetComponent<BoxCollider2D>();
      rb2D = GetComponent<Rigidbody2D>();
      inverseMoveTime = 1f / moveTime;
    }

    //Move returns true if it is able to move and false if not. 
    //Move takes parameters for x direction, y direction and a RaycastHit2D to check collision.
    bool Move(int xDir, int yDir, out RaycastHit2D hit)
    {
      //Store start position to move from, based on objects current transform position.
      Vector2 start = transform.position;

      // Calculate end position based on the direction parameters passed in when calling Move.
      Vector2 end = start + new Vector2(xDir, yDir);

      //Disable the boxCollider so that line cast doesn't hit this object's own collider.
      boxCollider.enabled = false;
      try
      {
        // We know that we are working in a regular grid. So we can avoid line-casting into
        // our own collider by simply starting to cast outside of it.
        var castingStart = Vector2.Lerp(start, end, 0.6f);
        //Cast a line from start point to end point checking collision on blockingLayer.
        hit = Physics2D.Linecast(castingStart, end, forceWalkableLayer);
        if (hit.transform != null)
        {
          // its ok to walk here, even though there is a block ..
          hit = default(RaycastHit2D);
          StartMoving(end);
          return true;
        }

        hit = Physics2D.Linecast(castingStart, end, blockingLayer);
      }
      finally
      {
        //Re-enable boxCollider after linecast
        boxCollider.enabled = true;
      }

      //Check if anything was hit
      if (hit.transform == null)
      {
        StartMoving(end);
        //Return true to say that Move was successful
        return true;
      }

      //If something was hit, return false, Move was unsuccesful.
      return false;
    }

    void StartMoving(Vector2 target)
    {
      Moving = true;
      OnMoveStarted.Invoke();
      //If nothing was hit, start SmoothMovement co-routine passing in the Vector2 end as destination
      StartCoroutine(SmoothMovement(target));
    }

    //Co-routine for moving units from one space to next, takes a parameter end to specify where to move to.
    protected IEnumerator SmoothMovement(Vector3 end)
    {
      //Calculate the remaining distance to move based on the square magnitude of the difference between current position and end parameter. 
      //Square magnitude is used instead of magnitude because it's computationally cheaper.
      float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

      //While that distance is greater than a very small amount (Epsilon, almost zero):
      while (sqrRemainingDistance > float.Epsilon)
      {
        //Find a new position proportionally closer to the end, based on the moveTime
        Vector3 newPostion = Vector3.MoveTowards(rb2D.position, end, inverseMoveTime * Time.deltaTime);

        //Call MovePosition on attached Rigidbody2D and move it to the calculated position.
        rb2D.MovePosition(newPostion);

        //Recalculate the remaining distance after moving.
        sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        //Return and loop until sqrRemainingDistance is close enough to zero to end the function
        yield return null;
      }

      Moving = false;
      OnMoveFinished.Invoke();
    }

    public bool AttemptMove(int xDir, int yDir, out Transform t)
    {
      //Hit will store whatever our linecast hits when Move is called.
      RaycastHit2D hit;

      //Set canMove to true if Move was successful, false if failed.
      if (Move(xDir, yDir, out hit))
      {
        t = default(Transform);
        return true;
      }

      // movement was blocked.
      var hitComponent = hit.transform;
      OnCantMove.Invoke();
      t = hitComponent;
      return false;
    }
  }
}
