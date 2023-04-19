using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class MagicManager : MonoBehaviour
{
  // public static MagicManager _instance = null;
  [SerializeField]
  private GameObject _game;

  [SerializeField]
  private GameObject _controlNode;
  [SerializeField]
  private GameObject _trainingUI;

  [SerializeField]
  private TMP_Dropdown trainingChoice;

  [SerializeField]
  private Casting _casting;
  private bool _isCasting;

  private TrainingProcessing _trainingProcessing = TrainingProcessing.Instance;

  // static public MagicManager Instance
  // {
  //     get {
  //         if (_instance == null)  {
  //             _instance = new MagicManager();
  //         }
  //         return _instance;
  //     }
  // }

  public void Deactivate()
  {
    _game.SetActive(true);
    Cursor.lockState = CursorLockMode.Locked;
    gameObject.SetActive(false);
  }

  public void CastingMode(bool casting)
  {
    _casting.Subscribe();
    _trainingUI.SetActive(!casting);
    _isCasting = casting;
  }

  private void Update() {
    if (_isCasting)
    {
      
    }
  }
}