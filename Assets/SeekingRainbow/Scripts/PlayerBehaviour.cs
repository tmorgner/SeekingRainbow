using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SeekingRainbow.Scripts
{
  [RequireComponent(typeof(MoveableBehaviour))]
  public class PlayerBehaviour : MonoBehaviour
  {
    public GameSession Session;
    MoveableBehaviour mover;
    List<ElementalBasePower> inventory;

    Vector2 touchOrigin = -Vector2.one; //Used to store location of screen touch origin for mobile controls.
    float pauseInputUntil;

    void Awake()
    {
      mover = GetComponent<MoveableBehaviour>();
    }

    void Update()
    {
      if (mover.Moving)
      {
        return;
      }

      var input = QueryInputs();
      if (input != Vector2Int.zero)
      {
        if (Session.SelectedAbilities.Count > 0)
        {
          // activate ability instead of moving.
          var ab = Session.SelectedAbilities.First();
          PerformAbility(ab, input);
          Session.Deselect(ab);
          pauseInputUntil = Time.time + 0.5f;
        }
        else if (pauseInputUntil > Time.time)
        {
          // sleep
        }
        else
        {
          Transform collision;
          if (!mover.AttemptMove(input.x, input.y, out collision))
          {
            // something happened.
          }
        }
      }
    }

    void PerformAbility(ElementalAbility ab, Vector2Int input)
    {
      var start = new Vector2Int((int) transform.position.x, (int) transform.position.y);
      AbilityEffect.PerformAbilityAt(ab, start, input);
    }

    Vector2Int QueryInputs()
    {
#if UNITY_STANDALONE || UNITY_WEBPLAYER
      return QueryInputStandalone();
#else
      return QueryInputTouch();
#endif
    }

    Vector2Int QueryInputTouch()
    {
      var horizontal = 0;
      var vertical = 0;
      //Check if Input has registered more than zero touches
      if (Input.touchCount > 0)
      {
        //Store the first touch detected.
        Touch myTouch = Input.touches[0];

        //Check if the phase of that touch equals Began
        if (myTouch.phase == TouchPhase.Began)
        {
          //If so, set touchOrigin to the position of that touch
          touchOrigin = myTouch.position;
        }

        //If the touch phase is not Began, and instead is equal to Ended and the x of touchOrigin is greater or equal to zero:
        else if (myTouch.phase == TouchPhase.Ended && touchOrigin.x >= 0)
        {
          //Set touchEnd to equal the position of this touch
          Vector2 touchEnd = myTouch.position;

          //Calculate the difference between the beginning and end of the touch on the x axis.
          float x = touchEnd.x - touchOrigin.x;

          //Calculate the difference between the beginning and end of the touch on the y axis.
          float y = touchEnd.y - touchOrigin.y;

          //Set touchOrigin.x to -1 so that our else if statement will evaluate false and not repeat immediately.
          touchOrigin.x = -1;

          //Check if the difference along the x axis is greater than the difference along the y axis.
          if (Mathf.Abs(x) > Mathf.Abs(y))
            //If x is greater than zero, set horizontal to 1, otherwise set it to -1
            horizontal = x > 0 ? 1 : -1;
          else
            //If y is greater than zero, set horizontal to 1, otherwise set it to -1
            vertical = y > 0 ? 1 : -1;
        }
      }

      return new Vector2Int(horizontal, vertical);
    }

    Vector2Int QueryInputStandalone()
    {
      //Get input from the input manager, round it to an integer and store in horizontal to set x axis move direction
      var horizontal = (int) (Input.GetAxisRaw("Horizontal"));

      //Get input from the input manager, round it to an integer and store in vertical to set y axis move direction
      var vertical = (int) (Input.GetAxisRaw("Vertical"));

      //Check if moving horizontally, if so set vertical to zero.
      if (horizontal != 0)
      {
        vertical = 0;
      }

      return new Vector2Int(horizontal, vertical);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
      var tileEffect = other.GetComponent<TileEffect>();
      tileEffect?.OnEffectTriggered(this);
    }
  }
}
