using UnityEngine;
using TMPro;
using EmotivUnityPlugin;
using System.Collections.Generic;
using System.Linq;

public class MagicManager : MonoBehaviour
{
  [SerializeField]
  private GameObject _game;
  [SerializeField]
  private ControlNode _controlNode;
  [SerializeField]
  private GameObject _trainingUI;
  [SerializeField]
  private TMP_Dropdown trainingChoice;
  [SerializeField]
  private TMP_Text _mcDisplayText;
  [SerializeField]
  private TMP_Text _patternText;
  [SerializeField]
  private TMP_Text _countdown;
  [SerializeField]
  private SubscribeTrain _subscribeTrain;
  [SerializeField]
  public static int trainingTimeLimit = 8;
  [SerializeField]
  private GameObject _trainingResultUI;
  [SerializeField]
  private TrainingResult _trainingResult;

  private static string _mcText = "";
  private static string action = "";
  private static double power = 0.0;
  private static double currentTime = 0.0;
  private static double lastUpdatedTime = 0.0;

  private int countTrained = 0;
  private double powerSum = 0.0;
  private double trainingStartTime = 0.0;
  private Spells.SpellSymbols trainingTarget;
  private bool _isTraining = false;
  private List<Spells.SpellSymbols> formula;
  private Dictionary<string, List<Spells.SpellSymbols>> currentMatching;
  private TrainingProcessing _trainingProcessing = TrainingProcessing.Instance;
  static readonly object _object = new object();  
  private void Awake() {
    _subscribeTrain.MentalUpdate += MentalUpdate;
  }

  private void OnDestroy() {
    _subscribeTrain.MentalUpdate -= MentalUpdate;
    _trainingProcessing.SaveCurProfile(_trainingProcessing.StaticHeadset.HeadsetID);
  }

  public void Deactivate()
  {
    ReturnToDefault();
    GameManager.Instance.ActivateGame(true);
    Cursor.lockState = CursorLockMode.Locked;
    GameManager.Instance.ActivateMagic(false);
  }

  public void Activate()
  {
    _subscribeTrain.Subscribe();
    ReturnToDefault();
  }

  public void BeginTraining()
  {
      _isTraining = true;
      countTrained = 0;
      powerSum = 0.0;
      trainingStartTime = currentTime;
      _subscribeTrain.StartTrain(trainingChoice.captionText.text.ToLower());
      trainingTarget = RecognizeSymbol(trainingChoice.captionText.text.ToLower());
      _controlNode.Training(trainingTarget);
  }
  private void Countdown()
  {
      if(currentTime - trainingStartTime >= trainingTimeLimit)
      {
        StopTraining();
        _countdown.text = 0.ToString();
      }
      else
      {
        _countdown.text = ((int)(trainingTimeLimit-(currentTime-trainingStartTime))).ToString();
      }
  }
  public void StopTraining()
  {
      Debug.Log("Training Complete");
      double score = powerSum / countTrained * 100;
      _trainingResult.DisplayScore(score);
      _trainingResultUI.SetActive(true);
      _isTraining = false;
      _controlNode.StopTraining();
  }
 
  public void AcceptTraining()
  {
      _subscribeTrain.StopTrain(true);
      Debug.Log(_trainingProcessing.StaticHeadset.HeadsetID);
      _trainingProcessing.SaveCurProfile(_trainingProcessing.StaticHeadset.HeadsetID);
      ReturnToDefault(true);
  }
  public void RejectTraining()
  {
      _subscribeTrain.StopTrain(false);
      _trainingProcessing.SaveCurProfile(_trainingProcessing.StaticHeadset.HeadsetID);
      ReturnToDefault(true);
  }
  public void ReturnToDefault(bool butPressed=false)
  {
      _mcText = "";
      action = "";
      power = 0.0;
      currentTime = 0.0;

      lastUpdatedTime = 0;
      countTrained = 0;
      powerSum = 0.0;
      trainingStartTime = 0.0;
      trainingTarget = Spells.SpellSymbols.None;
      _isTraining = false;
      formula = null;   
      _trainingResult.DisplayScore(0);

      if (_trainingResultUI.activeSelf && butPressed)
      {
        _trainingResultUI.SetActive(false);
        _subscribeTrain.StopTrain(false);
        _trainingProcessing.SaveCurProfile(_trainingProcessing.StaticHeadset.HeadsetID);
      }

      _controlNode.ResetPos();
  }

  private void MentalUpdate(object sender, MentalCommandEventArgs data)
  {
    lock(_object)
    {
      Debug.Log("Mental Update Received");
      action = data.Act;
      power = data.Pow;
      currentTime = data.Time;
      _mcText = action + " [" + power + "]";
    }
  }

  private Spells.SpellSymbols RecognizeSymbol(string symbol)
  {
    switch(symbol)
    {
      case "push":
        return Spells.SpellSymbols.Push;
      case "pull":
        return Spells.SpellSymbols.Pull;
      case  "right":
        return Spells.SpellSymbols.Right;
      case "left":
        return Spells.SpellSymbols.Left;
      // case "lift":
      //   return Spells.SpellSymbols.Lift;
      // case "drop":
      //   return Spells.SpellSymbols.Drop;
      default:
        return Spells.SpellSymbols.None;
    }
  }

  private bool AddSymbol(Spells.SpellSymbols symbol)
  {
    if (formula == null)
    {
      currentMatching = new Dictionary<string, List<Spells.SpellSymbols>>(Spells.Formula);
      formula = new List<Spells.SpellSymbols>();
    }
    bool matchFound = false;
    List<string> removeItems = new List<string>();
    
    //Check if recently added symbol fits the pattern
    foreach (KeyValuePair<string, List<Spells.SpellSymbols>> entry in currentMatching)
    {
      //Debug.Log(entry);
      int i = formula.Count;
      if(i >= entry.Value.Count)
      {
        continue;
      }
      else
      {
        if (entry.Value[i] == symbol)
        {
          matchFound = true;
        }
        else
        {
          removeItems.Add(entry.Key);
          
        }
      }
    }
    foreach(string k in removeItems)
    {
      currentMatching.Remove(k);
    }

    if(matchFound)
    {
      formula.Add(symbol);
      return true;
    }
    return false;
  }

  private void printFormula()
  {
    string text = "[";
    if (formula != null)
    {
      for(int i = 0; i < formula.Count; i++)
      {
        if (i == 0)
        {
          text += formula[i];
          continue;
        }
        text += ", " + formula[i];
      }
    }
    _patternText.text = text + "]";
  }
  

  public void ResetFormula()
  {
    formula = null;
    currentMatching = null;
  }
  
  public void LockInNode()
  {
    lock(_object)
    {
      if(_controlNode.CurrentNode == Spells.SpellSymbols.None)
      {
        _controlNode.ResetPos();
        return;
      }
      else if(_controlNode.CurrentNode == Spells.SpellSymbols.Neutral)
      {
        _controlNode.ResetPos();
        return;
      }
      else
      {
        if(!AddSymbol(_controlNode.CurrentNode))
        {
          ResetFormula();
        }
        _controlNode.ResetPos();
      }
    }
  }

  public void Cast()
  {
    lock(_object)
    {
      if(currentMatching.Count > 1)
      {
        foreach (KeyValuePair<string, List<Spells.SpellSymbols>> item in currentMatching)
        {
          if (MatchFormula(item.Value, formula))
          {
            SpellCasting.StoreSpell(item.Key);
            Deactivate();
            break;
          }
        }
      }
      if(currentMatching.Count == 1)
      {
        Debug.Log(currentMatching.ElementAt(0).Key);
        //_game.SetActive(true);
        SpellCasting.StoreSpell(currentMatching.ElementAt(0).Key);
        Deactivate();
      }
    }
  }

  private bool MatchFormula(List<Spells.SpellSymbols> spell, List<Spells.SpellSymbols> formula)
  {
      if (spell.Count != formula.Count)
        return false;
      for (int i = 0; i < spell.Count; i++)
      {

        if (formula[i] != spell[i])
        {
          return false;
        }
      }
      return true;
  }
  private void Update()
  {
    printFormula();
    _mcDisplayText.text = _mcText;
    lock (_object)
    {
      if (currentTime>lastUpdatedTime)
        {
          Spells.SpellSymbols sym = RecognizeSymbol(action);
          if(_isTraining)
          {
            Countdown();
            countTrained++;
            if (sym == trainingTarget)
              {powerSum += power;}
          }
          _controlNode.Move(sym, power);
          lastUpdatedTime = currentTime;
        }
      }
    }
    
}