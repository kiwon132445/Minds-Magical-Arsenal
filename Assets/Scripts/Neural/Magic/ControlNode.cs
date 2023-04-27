using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ControlNode : MonoBehaviour
{
  static readonly object _object = new object();
  Vector3 _position;
  public Rigidbody _rigid;
  
  [SerializeField]
  float neutralDirSpeed = 2f;
  [SerializeField]
  bool inNeutral = false;
  [SerializeField]
  float max_speed = 15f;
  [SerializeField]
  bool _isTraining = false;

  public Spells.SpellSymbols CurrentNode = Spells.SpellSymbols.Neutral;

  bool x_lock, y_lock, z_lock = false;
  bool neutral, push, pull, right, left, lift, drop;
  bool neutralTrain, pushTrain, pullTrain, rightTrain, leftTrain, liftTrain, dropTrain;
  const float MOVE_DELAY = 2.0f;
  const float RESET_DELAY = 5.0f;

  float _timerCounter_move_back = 0;
  float _timerCounter_reset = 0;

  private void OnTriggerEnter(Collider other)
  {
    Spells.SpellSymbols sym = TranslateNode(other.gameObject.name);
    if (sym == Spells.SpellSymbols.Neutral)
    {
      ResetPos();
    }
    else
      CurrentNode = sym;
  }
  private void OnTriggerExit(Collider other)
  {
    CurrentNode = Spells.SpellSymbols.None;
    Spells.SpellSymbols sym = TranslateNode(other.gameObject.name);
    if (sym == Spells.SpellSymbols.Neutral)
    {
      inNeutral = false;
    }
  }
  
  private void Start()
  {
    _position = gameObject.transform.localPosition;
    _rigid = gameObject.GetComponent<Rigidbody>();

    neutralTrain = pushTrain = pullTrain = rightTrain = leftTrain = liftTrain = dropTrain = true;
    neutral = push = pull = right = left = lift = drop = true;
  }

  private void Update() {
    CheckBoundary();
    _timerCounter_move_back += Time.deltaTime;
    if (_timerCounter_move_back > MOVE_DELAY) {
      _timerCounter_move_back -= MOVE_DELAY;
      _rigid.velocity = Vector3.zero;
      Neutral();
    }
    _timerCounter_reset += Time.deltaTime;
    if (_timerCounter_reset > RESET_DELAY) {
      _timerCounter_reset -= RESET_DELAY;
      _rigid.velocity = Vector3.zero;
      ResetPos();
    }
  }

  public void Move(Spells.SpellSymbols sym, double multiplier)
  {
    switch(sym)
    {
      case Spells.SpellSymbols.Neutral:
        Neutral();
        break;
      case Spells.SpellSymbols.Push:
        Push(((float)multiplier));
        break;
      case Spells.SpellSymbols.Pull:
        Pull(((float)multiplier));
        break;
      case Spells.SpellSymbols.Right:
        Right(((float)multiplier));
        break;
      case Spells.SpellSymbols.Left:
        Left(((float)multiplier));
        break;
      case Spells.SpellSymbols.Lift:
        Lift(((float)multiplier));
        break;
      case Spells.SpellSymbols.Drop:
        Drop(((float)multiplier));
        break;
      default:
        Neutral();
        break;
    }
  }

  public void Training(Spells.SpellSymbols target)
  {
    Debug.Log("Training Activated");
    neutralTrain = pushTrain = pullTrain = rightTrain = leftTrain = liftTrain = dropTrain = false;
    _isTraining = true;
    switch(target)
    {
      case Spells.SpellSymbols.Push:
        pushTrain = true;
        break;
      case Spells.SpellSymbols.Pull:
        pullTrain = true;
        break;
      case Spells.SpellSymbols.Right:
        rightTrain = true;
        break;
      case Spells.SpellSymbols.Left:
        leftTrain = true;
        break;
      case Spells.SpellSymbols.Lift:
        liftTrain = true;
        break;
      case Spells.SpellSymbols.Drop:
        dropTrain = true;
        break;
      default:
        neutralTrain = pushTrain = pullTrain = rightTrain = leftTrain = liftTrain = dropTrain = true;
        break;
    }
  }
  public void StopTraining()
  {
    neutralTrain = pushTrain = pullTrain = rightTrain = leftTrain = liftTrain = dropTrain = true;
    _isTraining = false;
  }

  public void ResetPos()
  {
    lock(_object)
    {
      CurrentNode = Spells.SpellSymbols.Neutral;
      _rigid.velocity = Vector3.zero;
      inNeutral = true;

      x_lock = y_lock = z_lock = false;
      gameObject.transform.localPosition = _position;
    }
  }
  public void CheckBoundary()
  {
    lock(_object)
    {
      // Check z boundary
      if (gameObject.transform.localPosition.z >= 5)
      {
        push=false;
        if(_rigid.velocity.z > 0)
        {
          _rigid.velocity = new Vector3(_rigid.velocity.x, _rigid.velocity.y, 0);
        }
      } else
      {
        push=true;
      }

      if (gameObject.transform.localPosition.z <= -5)
      {
        pull=false;
        if(_rigid.velocity.z < 0)
        {
          _rigid.velocity = new Vector3(_rigid.velocity.x, _rigid.velocity.y, 0);
        }
      } else
      {
        pull=true;
      }

      // Check x boundary
      if (gameObject.transform.localPosition.x >= 5)
      {
        right=false;
        if(_rigid.velocity.x > 0)
        {
          _rigid.velocity = new Vector3(0, _rigid.velocity.y, _rigid.velocity.z);
        }
      } else
      {
        right=true;
      }

      if (gameObject.transform.localPosition.x <= -5)
      {
        left=false;
        if(_rigid.velocity.x < 0)
        {
          _rigid.velocity = new Vector3(0, _rigid.velocity.y, _rigid.velocity.z);
        }
      } else
      {
        left=true;
      }

      // Check y boundary
      if (gameObject.transform.localPosition.y >= 5)
      {
        lift=false;
        if(_rigid.velocity.y > 0)
        {
          _rigid.velocity = new Vector3(_rigid.velocity.x, 0, _rigid.velocity.z);
        }
      } else
      {
        lift=true;
      }

      if (gameObject.transform.localPosition.y <= -5)
      {
        drop=false;
        if(_rigid.velocity.y < 0)
        {
          _rigid.velocity = new Vector3(_rigid.velocity.x, 0, _rigid.velocity.z);
        }
      } else
      {
        drop=true;
      }
    }
  }

  public void Neutral()
  {
    lock(_object)
    {
      if (inNeutral)
      {
        return;
      }
      Debug.Log("Neural Command activated");
      // Set x coordinates back to zero
      if (gameObject.transform.localPosition.x < _position.x)
      {
        _rigid.velocity = new Vector3(neutralDirSpeed, 0, 0);
      }
      else if (gameObject.transform.localPosition.x > _position.x)
      {
        _rigid.velocity = new Vector3(-neutralDirSpeed, 0, 0);
      }
      else
      {
        _rigid.velocity = new Vector3(0, _rigid.velocity.y, _rigid.velocity.z);
      }

      // Set y coordinates back to zero
      if (gameObject.transform.localPosition.y < _position.y)
      {
        _rigid.velocity = new Vector3(0, neutralDirSpeed, 0);
      }
      else if (gameObject.transform.localPosition.y > _position.y)
      {
        _rigid.velocity = new Vector3(0, -neutralDirSpeed, 0);
      }
      else
      {
        _rigid.velocity = new Vector3(_rigid.velocity.x, 0,_rigid.velocity.y);
      }

      // Set y coordinates back to zero
      if (gameObject.transform.localPosition.z < _position.z)
      {
        _rigid.velocity = new Vector3(0, 0, neutralDirSpeed);
      }
      else if (gameObject.transform.localPosition.z > _position.z)
      {
        _rigid.velocity = new Vector3(0, 0, -neutralDirSpeed);
      }
      else
      {
        _rigid.velocity = new Vector3(_rigid.velocity.x, _rigid.velocity.y, 0);
      }
    }
  }

  public void Push(float multiplier = 0.5f)
  {
    lock(_object)
    {
      
      if (push && pushTrain && (!z_lock || _isTraining))
      {
        _timerCounter_move_back = _timerCounter_reset = 0;
        if(!inNeutral)
        {
          gameObject.transform.localPosition = new Vector3(_position.x, _position.y, gameObject.transform.localPosition.z);
          x_lock = y_lock = true;
        }
        _rigid.velocity = new Vector3(0, 0, multiplier*max_speed);
      }
      else if (!push)
      {
        _timerCounter_move_back = _timerCounter_reset = 0;
        if(_rigid.velocity.z > 0)
        {
          _rigid.velocity = new Vector3(_rigid.velocity.x, _rigid.velocity.y, 0);
        }
      }
    }
  }
  public void Pull(float multiplier = 0.5f)
  {
    lock(_object)
    {
      if (pull && pullTrain && (!z_lock || _isTraining))
      {
        _timerCounter_move_back = _timerCounter_reset = 0;

        if(!inNeutral)
        {
          gameObject.transform.localPosition = new Vector3(_position.x, _position.y, gameObject.transform.localPosition.z);
          x_lock = y_lock = true;
        }
        _rigid.velocity = new Vector3(0, 0, -multiplier*max_speed);
      }
      else if (!pull)
      {
        _timerCounter_move_back = _timerCounter_reset = 0;
        if(_rigid.velocity.z < 0)
        {
          _rigid.velocity = new Vector3(_rigid.velocity.x, _rigid.velocity.y, 0);
        }
      }
    }
  }
  public void Right(float multiplier = 0.5f)
  {
    lock(_object)
    {
      if (right && rightTrain && (!x_lock || _isTraining))
      {
        _timerCounter_move_back = _timerCounter_reset = 0;

        if(!inNeutral)
        {
          gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, _position.y, _position.z);
          y_lock = z_lock = true;
        }
        _rigid.velocity = new Vector3(multiplier*max_speed, 0, 0);
      }
      else if (!right)
      {
        _timerCounter_move_back = _timerCounter_reset = 0;
        if(_rigid.velocity.x > 0)
        {
          _rigid.velocity = new Vector3(0, _rigid.velocity.y, _rigid.velocity.z);
        }
      }
    }
  }
  public void Left(float multiplier = 0.5f)
  {
    lock(_object)
    {
      if (left && leftTrain && (!x_lock || _isTraining))
      {
        _timerCounter_move_back = _timerCounter_reset = 0;
        if(!inNeutral)
        {
          gameObject.transform.localPosition = new Vector3(gameObject.transform.localPosition.x, _position.y, _position.z);
          y_lock = z_lock = true;
        }
        _rigid.velocity = new Vector3(-multiplier*max_speed, 0, 0);
      }
      else if (!left)
      {
        _timerCounter_move_back = _timerCounter_reset = 0;
        if(_rigid.velocity.x < 0)
        {
          _rigid.velocity = new Vector3(0, _rigid.velocity.y, _rigid.velocity.z);
        }
      }
    }
  }
  public void Lift(float multiplier = 0.5f)
  {
    lock(_object)
    {
      if (lift && liftTrain && (!y_lock || _isTraining))
      {
        _timerCounter_move_back = _timerCounter_reset = 0;
        if(!inNeutral)
        {
          gameObject.transform.localPosition = new Vector3(_position.x, gameObject.transform.localPosition.y, _position.z);
          x_lock = z_lock = true;
        }
        _rigid.velocity = new Vector3(0, multiplier*max_speed, 0);
      }
      else if (!lift)
      {
        _timerCounter_move_back = _timerCounter_reset = 0;
        if(_rigid.velocity.y > 0)
        {
          _rigid.velocity = new Vector3(_rigid.velocity.x, 0, _rigid.velocity.z);
        }
      }
    }
  }
  public void Drop(float multiplier = 0.5f)
  {
    lock(_object)
    {
      if (drop && dropTrain && (!y_lock || _isTraining))
      {
        _timerCounter_move_back = _timerCounter_reset = 0;
        if(!inNeutral)
        {
          gameObject.transform.localPosition = new Vector3(_position.x, gameObject.transform.localPosition.y, _position.z);
          x_lock = z_lock = true;
        }
        _rigid.velocity = new Vector3(0, -multiplier*max_speed, 0);
      }
      else if (!drop)
      {
        _timerCounter_move_back = _timerCounter_reset = 0;
        if(_rigid.velocity.y < 0)
        {
          _rigid.velocity = new Vector3(_rigid.velocity.x, 0, _rigid.velocity.z);
        }
      }
    }
  }

  private Spells.SpellSymbols TranslateNode(string node)
  {
    switch(node)
    {
      case "NeutralNode":
        return Spells.SpellSymbols.Neutral;
      case "PushNode":
        return Spells.SpellSymbols.Push;
      case "PullNode":
        return Spells.SpellSymbols.Pull;
      case "RightNode":
        return Spells.SpellSymbols.Right;
      case "LeftNode":
        return Spells.SpellSymbols.Left;
      case "LiftNode":
        return Spells.SpellSymbols.Lift;
      case "DropNode":
        return Spells.SpellSymbols.Drop;
      default:
        return Spells.SpellSymbols.None;
    }
  }
}