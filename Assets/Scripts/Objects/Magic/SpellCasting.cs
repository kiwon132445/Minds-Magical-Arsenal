using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EmotivUnityPlugin;
using TMPro;
using System.Linq;

public class SpellCasting : MonoBehaviour
{
    [SerializeField] StarterAssets.FirstPersonController _fPController;
    [SerializeField] SelectionManager _selectionManager;
    [SerializeField] StarterAssets.StarterAssetsInputs _input;
    [SerializeField] FireProjectiles _fireProj;
    [SerializeField] SubscribeTrain _subscribeTrain;
    [SerializeField] TMP_Text spellText;
    [SerializeField] TMP_Text mentalCommandText;
    [SerializeField] TMP_Text cooldownText;
    [SerializeField] float max_force = 10.0f;
    [SerializeField] GameObject aura;
    public static string currentSpell = "Telekinesis";
    static readonly object _object = new object();  

    private float _curSpellCD;
    private Spells.SpellElement _element;
    private Spells.SpellType _type;

    private bool _spellOnCD;
    private bool _spellActive;
    private ForceMode forceType = ForceMode.Force;

    #region Neural
    private bool subscribed;
    private static string _mcText = "";
    private static string action = "";
    private static double power = 0.5;
    private double cooldownStartTime = 0;
    private double cooldownStatus = 0;
    #endregion
    
    private void Awake() {
        _subscribeTrain.MentalUpdate += MentalUpdate;
    }

    private void OnDestroy() {
        _subscribeTrain.MentalUpdate -= MentalUpdate;
    }
    
    public static void StoreSpell(string spellName)
    {
        lock(_object)
        { 
            currentSpell = spellName;
        }
    }

    public void ActivateSpell(bool status)
    {
        lock(_object)
        {
            if (!subscribed)
            {
                if(TrainingProcessing.Instance.IsProfileConnected())
                {
                    subscribed = true;
                    _subscribeTrain.Subscribe();
                }

                //Default values if not subscribed for testing
                power = 0.5;
                action = "pull";
                _mcText = "pull";
            }
            _spellActive = status;
        }
    }

    private void Update() {
        
        spellText.text = "Current Spell: " + currentSpell;
        mentalCommandText.text = "Mental Command: " + _mcText;
        
        if (!string.IsNullOrWhiteSpace(currentSpell))
        {
            if (_spellOnCD && _curSpellCD != 0)
            {
                cooldownStatus += Time.deltaTime;
                if (cooldownStatus-cooldownStartTime >= _curSpellCD) {
                    Debug.Log("Cooldown Finished");
                    _spellOnCD = false;
                    cooldownText.text = "Ready";
                }
                else
                {
                    cooldownText.text = ((int) (_curSpellCD - cooldownStatus)).ToString();
                }
            }
            else
            {
                cooldownText.text = "Ready";
                _spellOnCD = false;
            }
            
            
            if (_spellActive)
            {
                Debug.Log("Passed spell activation");
                if (!_spellOnCD)
                {
                    Debug.Log("Passed spell cooldown");
                    RetrieveSpellEffects();
                    ActivateSpell();
                    _spellOnCD = true;
                    cooldownStatus = 0;
                    cooldownStartTime = Time.deltaTime;
                }
            }
        }
        if (!_spellActive)
        {
            Debug.Log("Spell not active");
            if(aura.activeSelf)
                aura.SetActive(false);
            _selectionManager.ObjectInUse = false;
            _fPController.Telekinesis = false;
            _fPController.Flying = false;
        }
    }

    private void ResetSpellStatus()
    {
        lock(_object)
        {
            _spellActive = false;
            _spellOnCD = false;
            _selectionManager.ObjectInUse = false;
            _fPController.Telekinesis = false;
            _fPController.Flying = false;
        }
    }

    private void RetrieveSpellEffects()
    {
        lock(_object)
        {
            if (!Spells.SpellEffectDictionary.ContainsKey(currentSpell))
            {
                Debug.Log("Spell not found");
                return;
            }
            
            Spells.SpellEffect effects = Spells.SpellEffectDictionary[currentSpell];
            _curSpellCD = effects.cooldown;
            _element = effects.element;
            _type = effects.type;
        }
    }

    private void ActivateSpell()
    {

            switch(_type)
            {
                case Spells.SpellType.Arrow:
                    SpawnProjectile();
                    break;
                case Spells.SpellType.Ball:
                    SpawnProjectile();
                    break;
                case Spells.SpellType.Aura:
                    Aura();
                    break;
                case Spells.SpellType.Telekinesis:
                    _fPController.Telekinesis = true;
                    Telekinesis(false);
                    break;
                case Spells.SpellType.Flight:
                    _fPController.Flying = true;
                    Telekinesis(true);
                    break;
            }

    }

    private void SpawnProjectile()
    {
        Debug.Log("Firing Projectile.");
        _fireProj.FireProjectile(currentSpell);
    }

    private void Aura()
    {

    }

    private void Telekinesis(bool self=false)
    {
        lock(_object)
        {
            if (!aura.activeSelf)
                aura.SetActive(true);
            if (self)
            {
                Debug.Log("Flying");
                Spells.SpellSymbols sym = RecognizeSymbol(action);
                Move(gameObject, self, sym, power);
            }
            else
            {
                _selectionManager.ObjectInUse = true;
                Debug.Log("Telekinesis");
                if (_selectionManager.SelectedObject != null)
                {
                    Spells.SpellSymbols sym = RecognizeSymbol(action);
                    Move(_selectionManager.SelectedObject, self, sym, power);
                }
            }
        }
        
    }

    #region Neural spells
    private void Move(GameObject target, bool self, Spells.SpellSymbols sym, double multiplier)
    {
        switch(sym)
        {
            case Spells.SpellSymbols.Neutral:
                //Neutral(target, self);
                break;
            case Spells.SpellSymbols.Push:
                Push(target, self, ((float)multiplier));
                break;
            case Spells.SpellSymbols.Pull:
                Pull(target, self, ((float)multiplier));
                break;
            case Spells.SpellSymbols.Right:
                Right(target, self, ((float)multiplier));
                break;
            case Spells.SpellSymbols.Left:
                Left(target, self, ((float)multiplier));
                break;
            case Spells.SpellSymbols.Lift:
                Lift(target, self, ((float)multiplier));
                break;
            case Spells.SpellSymbols.Drop:
                if(!_fPController.Grounded || !self)
                    Drop(target, self, ((float)multiplier));
                break;
            default:
                break;
        }
        if (_input.jump)
        {
            Lift(target, self, 0.5f);
        }
        else if(_input.drop)
        {
            if(!_fPController.Grounded || !self)
                Drop(target, self, 0.5f);
        }
    }

    private void MentalUpdate(object sender, MentalCommandEventArgs data)
    {
        lock(_object)
        {
            Debug.Log("Mental Command Received");
            action = data.Act;
            power = data.Pow;
            _mcText = action + " [" + power + "]";
        }
    }

    private Spells.SpellSymbols RecognizeSymbol(string symbol)
  {
    switch(symbol.ToLower())
    {
      case "push":
        return Spells.SpellSymbols.Push;
      case "pull":
        return Spells.SpellSymbols.Pull;
      case  "right":
        return Spells.SpellSymbols.Right;
      case "left":
        return Spells.SpellSymbols.Left;
      case "lift":
        return Spells.SpellSymbols.Lift;
      case "drop":
        return Spells.SpellSymbols.Drop;
      default:
        return Spells.SpellSymbols.None;
    }
  }
  public void Neutral(GameObject target, bool self)
  {
    lock(_object)
    {
        if (self)
        {
            CharacterController controller = target.GetComponent<CharacterController>();
            controller.Move(Vector3.zero);
        }
        else
        {
            Rigidbody rigid = target.GetComponent<Rigidbody>();
            rigid.AddForce(Vector3.zero, forceType);
        }
    }
  }

  public void Push(GameObject target, bool self, float multiplier = 0.5f)
  {
    lock(_object)
    {
        if (self)
        {
            CharacterController controller = target.GetComponent<CharacterController>();
            controller.Move(gameObject.transform.forward * multiplier*max_force * Time.deltaTime);
        }
        else
        {
            Rigidbody rigid = target.GetComponent<Rigidbody>();
            rigid.AddForce(gameObject.transform.forward * multiplier*max_force, forceType);
        }
    }
  }
  public void Pull(GameObject target, bool self, float multiplier = 0.5f)
  {
    lock(_object)
    {
        if (self)
        {
            CharacterController controller = target.GetComponent<CharacterController>();
            controller.Move(-gameObject.transform.forward * multiplier*max_force * Time.deltaTime);
        }
        else
        {
            Rigidbody rigid = target.GetComponent<Rigidbody>();
            rigid.AddForce(-gameObject.transform.forward * multiplier*max_force, forceType);
        }
    }
  }
  public void Right(GameObject target, bool self, float multiplier = 0.5f)
  {
    lock(_object)
    {
        if (self)
        {
            CharacterController controller = target.GetComponent<CharacterController>();
            controller.Move(gameObject.transform.right * multiplier*max_force * Time.deltaTime);
        }
        else
        {
            Rigidbody rigid = target.GetComponent<Rigidbody>();
            rigid.AddForce(gameObject.transform.right * multiplier*max_force, forceType);
        }
    }
  }
  public void Left(GameObject target, bool self, float multiplier = 0.5f)
  {
    lock(_object)
    {
        if (self)
        {
            CharacterController controller = target.GetComponent<CharacterController>();
            controller.Move(-gameObject.transform.right * multiplier*max_force * Time.deltaTime);
        }
        else
        {
            Rigidbody rigid = target.GetComponent<Rigidbody>();
            rigid.AddForce(-gameObject.transform.right * multiplier*max_force, forceType);
        }
    }
  }
  public void Lift(GameObject target, bool self, float multiplier = 0.5f)
  {
    lock(_object)
    {
        if (self)
        {
            CharacterController controller = target.GetComponent<CharacterController>();
            controller.Move(Vector3.up * multiplier*max_force * Time.deltaTime);
        }
        else
        {
            Rigidbody rigid = target.GetComponent<Rigidbody>();
            rigid.AddForce(Vector3.up * multiplier*max_force, forceType);
        }
    }
  }
  public void Drop(GameObject target, bool self, float multiplier = 0.5f)
  {
    lock(_object)
    {
        if (self)
        {
            CharacterController controller = target.GetComponent<CharacterController>();
            controller.Move(Vector3.down * multiplier*max_force * Time.deltaTime);
        }
        else
        {
            Rigidbody rigid = target.GetComponent<Rigidbody>();
            rigid.AddForce(Vector3.down * multiplier*max_force, forceType);
        }
    }
  }
  #endregion
}
