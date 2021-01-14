using System.Collections.Generic;
using System.Collections;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine.XR.ARCore;
#endif

/// <summary>
/// This component uses the ARCoreFaceSubsystem to query for face regions, special
/// regions detected within a face, such as the nose tip. Each region has a pose
/// associated with it. This component instantiates <see cref="regionPrefab"/>
/// at each detected region.
/// </summary>
[RequireComponent(typeof(ARFaceManager))]
[RequireComponent(typeof(ARSessionOrigin))]
public class ARCoreFaceRegionManager : MonoBehaviour
{
    GameObject m_RegionPrefab;

    /// <summary>
    /// Get or set the prefab which will be instantiated at each detected face region.
    /// </summary>
    public GameObject regionPrefab
    {
        get { return m_RegionPrefab; }
        set { m_RegionPrefab = value; }
    }

    ARFaceManager m_FaceManager;
    int flag = 0;
    int anim = 0;
    Vector3 pozitiedefault;
    Quaternion rotatiedefault;
    GameObject go;
    float offset;
    float offsetRot;
    public Button santa_hat_btn;
    public Button barba_btn;
    public Button ochelari_bogdan_btn;
    public Button ochelari_rahela_btn;
    public Button coarne_rahela_btn;
    public Button ochelari_stefan_btn;
    public Button gift_btn;
    public Button ochelari_dragos_btn;


    ARSessionOrigin m_SessionOrigin;

#if UNITY_ANDROID && !UNITY_EDITOR
    NativeArray<ARCoreFaceRegionData> m_FaceRegions;

    Dictionary<TrackableId, Dictionary<ARCoreFaceRegion, GameObject>> m_InstantiatedPrefabs;
#endif

    // Start is called before the first frame update
    void Start()
    {
        
        santa_hat_btn.onClick.AddListener(delegate { MaskOn("https://drive.google.com/uc?export=download&id=1vjy8F5O7GLJSNxMHb4doO1q9eixcSZ8R"); });
        barba_btn.onClick.AddListener(delegate { MaskOn("https://drive.google.com/uc?export=download&id=1oXmLaZBqhN4y8XP0LOyXDCbpDBzgBNJh"); });
        ochelari_bogdan_btn.onClick.AddListener(delegate { MaskOn("https://drive.google.com/uc?export=download&id=14-qn0DPIDmdhJtkADsj7i8UermwS7qRL"); });
        ochelari_rahela_btn.onClick.AddListener(delegate { MaskOn("https://drive.google.com/uc?export=download&id=1NCD2298euXVzhQBuYz7OSknhmhN7Mxev"); });
        coarne_rahela_btn.onClick.AddListener(delegate { MaskOn("https://drive.google.com/uc?export=download&id=1aIkP6Y5gvamiOpQLi8B1-Rk61-fdh9zk"); });
        ochelari_stefan_btn.onClick.AddListener(delegate { MaskOn("https://drive.google.com/uc?export=download&id=1C9HMMrK3U9-n7Vpmz_jTHNmhd7n3YZ-3"); });
        gift_btn.onClick.AddListener(delegate { MaskOn("https://drive.google.com/uc?export=download&id=1udOxYcO_IlJigSNC-s38kCFl946z-pUF"); });
        ochelari_dragos_btn.onClick.AddListener(delegate { MaskOn("https://drive.google.com/uc?export=download&id=1IRlARuF5L_olcSw0DXX2ndqVURl1wE4-"); });


        m_FaceManager = GetComponent<ARFaceManager>();
        m_SessionOrigin = GetComponent<ARSessionOrigin>();
#if UNITY_ANDROID && !UNITY_EDITOR
        m_InstantiatedPrefabs = new Dictionary<TrackableId, Dictionary<ARCoreFaceRegion, GameObject>>();
#endif
    }
 
    void MaskOn(string urlLink)
    {
        StartCoroutine(webReq(urlLink));
    }

    // Update is called once per frame
    void Update()
    {
        //score.text = "buza sus: " + face.vertices[1].y + " buza jos: " + face.vertices[14].y;
        //

#if UNITY_ANDROID && !UNITY_EDITOR
        var subsystem = (ARCoreFaceSubsystem)m_FaceManager.subsystem;
        if (subsystem == null)
            return;

        foreach (var face in m_FaceManager.trackables)
        {
            Dictionary<ARCoreFaceRegion, GameObject> regionGos;
            if (!m_InstantiatedPrefabs.TryGetValue(face.trackableId, out regionGos))
            {
                regionGos = new Dictionary<ARCoreFaceRegion, GameObject>();
                m_InstantiatedPrefabs.Add(face.trackableId, regionGos);
            }

            subsystem.GetRegionPoses(face.trackableId, Allocator.Persistent, ref m_FaceRegions);
            
                var regionType = m_FaceRegions[0].region;
                if( anim == 1 && face.vertices[14].y < -0.073 ){
                    go.transform.localPosition = m_FaceRegions[0].pose.position + pozitiedefault + new Vector3(0,-0.1f,offset); 
                    go.transform.localRotation = m_FaceRegions[0].pose.rotation * rotatiedefault * Quaternion.Euler(0, 0, offsetRot);
                    offset -= 0.02f;
                    offsetRot += 15;
                    if( offset < -0.4f){
                        offset = 0.3f;
                    }
                    if( offsetRot > 355){
                        offsetRot = 0;
                    }

                }else if( anim == 1 ) {
                    go.transform.localPosition = m_FaceRegions[0].pose.position + pozitiedefault + new Vector3(0,0.8f,0); 
                    go.transform.localRotation = m_FaceRegions[0].pose.rotation * rotatiedefault;
                    offset = 0.3f;
                }else{
                        go.transform.localPosition = m_FaceRegions[0].pose.position + pozitiedefault; 
                        go.transform.localRotation = m_FaceRegions[0].pose.rotation * rotatiedefault;
                }
        }
        
#endif
    }

    void OnDestroy()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (m_FaceRegions.IsCreated)
            m_FaceRegions.Dispose();
#endif
    }

    IEnumerator webReq(string urlLink)
    {
        if( anim == 1)
        {
            anim = 0;
        }
        if (urlLink == "https://drive.google.com/uc?export=download&id=1udOxYcO_IlJigSNC-s38kCFl946z-pUF")
        {
            anim = 1;
            offset = 0.5f;
            offsetRot = 5;
        }
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(urlLink);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("Network error");
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            if (bundle != null)
            {
                if (go != null) { go.SetActive(false); }
                string rootAssetPath = bundle.GetAllAssetNames()[0];
                Debug.Log("numele fisier" + rootAssetPath);
                m_RegionPrefab = (GameObject)bundle.LoadAsset(rootAssetPath);
                go = Instantiate(m_RegionPrefab, m_SessionOrigin.trackablesParent);
                bundle.Unload(false);
                go.SetActive(true);
                rotatiedefault = m_RegionPrefab.transform.localRotation;
                pozitiedefault = m_RegionPrefab.transform.localPosition;
                flag = 1 - flag;
            }
            else
            {
                Debug.LogError("Not a valid asset bundle");
            }
        }
    }
}
