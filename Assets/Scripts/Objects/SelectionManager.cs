using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    public Camera mainCamera;
    [SerializeField] private string selectableTag = "Object";
    [SerializeField] private Color emissionColor = Color.white;

    public bool ObjectInUse;
    private GameObject _curGameObject;
    public GameObject SelectedObject   // property
  {
    get { return _curGameObject; }   // get method
    set { _curGameObject = value; }  // set method
  }

    // Update is called once per frame
    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
          GameObject selected = hit.transform.gameObject;
          if (selected.CompareTag(selectableTag))
          {
            if (!ObjectInUse && _curGameObject != selected)
            {
              UnSelect();
              _curGameObject = selected;
            }
              
            ShowSelect();
            Debug.Log(_curGameObject.name);
          }
          else if(!ObjectInUse)
          {
            UnSelect();
          }
          
        }
        else if(!ObjectInUse)
        {
          UnSelect();
        }
    }

    void ShowSelect()
    {
      Renderer selectedRenderer = _curGameObject.GetComponent<Renderer>();
      if (selectedRenderer != null)
      {
        selectedRenderer.material.EnableKeyword("_EMISSION");
        selectedRenderer.material.SetColor("_EmissionColor", emissionColor);
      }
    }

    void UnSelect()
    {
      if (_curGameObject != null)
      {
        Renderer selectedRenderer = _curGameObject.GetComponent<Renderer>();
        if (selectedRenderer != null)
        {
          selectedRenderer.material.DisableKeyword("_EMISSION");
        }
        _curGameObject = null;
        }
    }
}
