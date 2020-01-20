using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{

    public HexMap hexMap;
    public GameObject expansionPopupPanel;
    public float speed;
    public Material ChunkPreviewSelected;
    public Material ChunkPreviewUnselected;

    private string ChunkPreviewTag = "ChunkPreview";
    private string HexTileTag = "HexTile";
    private Transform currentSelected;
    private bool selectable = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Panning
        float horiz = Input.GetAxis("Horizontal");
        float vert = Input.GetAxis("Vertical");
        this.transform.position +=((Vector3.forward * vert + Vector3.right *horiz) * speed* Time.deltaTime);

        //Zooming
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
        if (mouseWheel != 0)
        {
            this.transform.position += (Vector3.up * mouseWheel) * speed;
        }

        //mouse hover
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(r, out hit) && selectable)
        {
            // Debug.Log(hit.transform.name);

            //highlight chunk previews
            if (hit.transform.CompareTag(ChunkPreviewTag))
            {
                MeshRenderer selectedMR = hit.transform.GetComponentInChildren<MeshRenderer>();
                selectedMR.material = ChunkPreviewSelected;
                
            }
            if (currentSelected != null && currentSelected.CompareTag(ChunkPreviewTag) &&currentSelected != hit.transform)
            {
                    MeshRenderer currentSelectedMR = currentSelected.GetComponentInChildren<MeshRenderer>();
                    currentSelectedMR.material = ChunkPreviewUnselected;
            }
            //click on chunk preview
            if (hit.transform.CompareTag(ChunkPreviewTag) && Input.GetButtonDown("Fire1"))
            {
                expansionPopupPanel.SetActive(true);
                selectable = false;
            }
            
            currentSelected = hit.transform;
         }
    }
    //called when yes button is pushed on expansion popup panel
    //deletes selected preview and generates new chunk and new chunk =previews
    public void ExpansionPopupPanelYes()
    {
        hexMap.GenerateChunkFromPreview(currentSelected.parent.gameObject);
        expansionPopupPanel.SetActive(false);
        selectable = true;
    }
    //called when no button is pushed on expansion popup panel closes panel
    public void ExpansionPopupPanelNo()
    {
        expansionPopupPanel.SetActive(false);
        selectable = true;
    }

}
