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
public class GameManager : MonoBehaviour
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
    Vector3 pozitiemisto;
    Vector3 pozitie_initiala_cap;
    Quaternion rotatiemisto;
    GameObject go;
    GameObject go2;
    GameObject go3;
    float offset;
    float offset2;
    float offset3;
    float offsetRot;
    bool initializare = false;
    bool startGame = false;
    public static int scoreValue = 0;
    public Text score;

    ARSessionOrigin m_SessionOrigin;

#if UNITY_ANDROID && !UNITY_EDITOR
    NativeArray<ARCoreFaceRegionData> m_FaceRegions;

    Dictionary<TrackableId, Dictionary<ARCoreFaceRegion, GameObject>> m_InstantiatedPrefabs;
#endif

    // Start is called before the first frame update
    void Start()
    {
        score.text = "Score: " + scoreValue;
        pozitie_initiala_cap = new Vector3(0, 0, 0);
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
        if (Input.GetMouseButton(0) && startGame == false)
        {
            MaskOn("https://drive.google.com/uc?export=download&id=1udOxYcO_IlJigSNC-s38kCFl946z-pUF");
        }

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
            if(initializare == true){
                pozitie_initiala_cap = m_FaceRegions[0].pose.position;
                initializare = false;
            }
                var regionType = m_FaceRegions[0].region;
                if( anim == 1 && startGame == true){
                    go.transform.localPosition = pozitie_initiala_cap + pozitiemisto + new Vector3(0,offset,0); 
                    go.transform.localRotation = m_FaceRegions[0].pose.rotation * rotatiemisto * Quaternion.Euler(0, 0, offsetRot);
                    go2.transform.localPosition = pozitie_initiala_cap + pozitiemisto + new Vector3(-0.1f,offset2,0); 
                    go2.transform.localRotation = m_FaceRegions[0].pose.rotation * rotatiemisto * Quaternion.Euler(0, 0, offsetRot);
                    go3.transform.localPosition = pozitie_initiala_cap + pozitiemisto + new Vector3(0.1f,offset3,0); 
                    go3.transform.localRotation = m_FaceRegions[0].pose.rotation * rotatiemisto * Quaternion.Euler(0, 0, offsetRot);
                    offset -= 0.01f;
                    offset2 -= 0.01f;
                    offset3 -= 0.01f;
                    offsetRot += 5;
                    if(go.transform.localPosition.y >= m_FaceRegions[0].pose.position.y - 0.05f && go.transform.localPosition.y <= m_FaceRegions[0].pose.position.y + 0.05f && go.transform.localPosition.x >= m_FaceRegions[0].pose.position.x - 0.05f && go.transform.localPosition.x <= m_FaceRegions[0].pose.position.x + 0.05f){
                        offset = 0.4f;
                        scoreValue += 10;
                        score.text = "Score: " + scoreValue;
                    }else if(go2.transform.localPosition.y >= m_FaceRegions[0].pose.position.y - 0.05f && go2.transform.localPosition.y <= m_FaceRegions[0].pose.position.y + 0.05f && go2.transform.localPosition.x >= m_FaceRegions[0].pose.position.x - 0.05f && go2.transform.localPosition.x <= m_FaceRegions[0].pose.position.x + 0.05f){
                        offset2 = 0.7f;
                        scoreValue += 10;
                        score.text = "Score: " + scoreValue;
                    }else if(go3.transform.localPosition.y >= m_FaceRegions[0].pose.position.y - 0.05f && go3.transform.localPosition.y <= m_FaceRegions[0].pose.position.y + 0.05f && go3.transform.localPosition.x >= m_FaceRegions[0].pose.position.x - 0.05f && go3.transform.localPosition.x <= m_FaceRegions[0].pose.position.x + 0.05f){
                        offset3 = 0.8f;
                        scoreValue += 10;
                        score.text = "Score: " + scoreValue;
                    }

                    if( offset < -0.2f){
                        score.text = "GAME OVER! YOUR SCORE: " + scoreValue; 
                        startGame = false;
                        scoreValue = 0;
                    }
                    if( offset2 < -0.2f){
                        score.text = "GAME OVER! YOUR SCORE: " + scoreValue;
                        startGame = false;
                        scoreValue = 0;
                    }
                    if( offset3 < -0.2f){
                        startGame = false;                        
                        score.text = "GAME OVER! YOUR SCORE: " + scoreValue;
                        scoreValue = 0;
                    }
                    if( offsetRot > 355){
                        offsetRot = 0;
                    }

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
        if (anim == 1)
        {
            anim = 0;
        }
        if (urlLink == "https://drive.google.com/uc?export=download&id=1udOxYcO_IlJigSNC-s38kCFl946z-pUF")
        {
            anim = 1;
            offset = 0.5f;
            offset2 = 0.5f;
            offset3 = 0.5f;
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
                if (go != null && go2 != null && go3 != null) { go.SetActive(false); go2.SetActive(false); go3.SetActive(false); }
                string rootAssetPath = bundle.GetAllAssetNames()[0];
                Debug.Log("numele fisier" + rootAssetPath);
                m_RegionPrefab = (GameObject)bundle.LoadAsset(rootAssetPath);
                go = Instantiate(m_RegionPrefab, m_SessionOrigin.trackablesParent);
                go2 = Instantiate(m_RegionPrefab, m_SessionOrigin.trackablesParent);
                go3 = Instantiate(m_RegionPrefab, m_SessionOrigin.trackablesParent);
                bundle.Unload(false);
                go.SetActive(true);
                go2.SetActive(true);
                go3.SetActive(true);
                rotatiemisto = m_RegionPrefab.transform.localRotation;
                pozitiemisto = m_RegionPrefab.transform.localPosition;
                flag = 1 - flag;
                initializare = true;
                startGame = true;
            }
            else
            {
                Debug.LogError("Not a valid asset bundle");
            }
        }
    }
}
