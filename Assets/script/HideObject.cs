using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class HideObject : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject m_root;
    public string[] m_names;
    public GameObject[] m_cols;
    public GameObject[] m_objs;
    public Vector3[] m_poss;
    public bool[] m_shows;
    protected AsyncOperationHandle<GameObject>[] m_handles = new AsyncOperationHandle<GameObject>[2];
    void Start()
    {
        //InstantiateAsync(0);
        //InstantiateAsync(1);
    }

    async void InstantiateAsync(int _idx)
    {
        Vector3 position = m_poss[_idx];
        Quaternion rotation = Quaternion.identity;
        string label = m_names[_idx];
        m_handles[_idx] = Addressables.InstantiateAsync(label, position, rotation); //Addressables.LoadAssetAsync<GameObject>(label);
        Debug.Log("..holding");
        await m_handles[_idx].Task;
        if (m_handles[_idx].Status == AsyncOperationStatus.Succeeded)
        {
            //m_objs[_idx] = Instantiate(m_handles[_idx].Result);
            m_objs[_idx] = m_handles[_idx].Result;
            m_objs[_idx].SetActive(true);
            m_objs[_idx].transform.parent = m_root.transform;
            m_objs[_idx].transform.position = m_poss[_idx];
            // Use the instantiated prefab as needed
            Debug.Log("Instantiated prefab: " + m_objs[_idx].name);
        }
        else
        {
            Debug.LogError("Failed to instantiate prefab: " + label);
        }
        //Addressables.ReleaseInstance(m_handles[_idx]);
    }

    public bool IsInVolume(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        Bounds bounds = renderer.bounds;
        Camera main = Camera.main;
        Plane[] frustum = GeometryUtility.CalculateFrustumPlanes(main);

        if (GeometryUtility.TestPlanesAABB(frustum, bounds))
            return true;
        return false;
    }

    public bool IsInVolume(int _idx)
    {
        return IsInVolume(m_cols[_idx]);
    }

    public void ShowHide(int _idx)
    {
        if (IsInVolume(_idx))//IsInVolume(m_objs[_idx]))
        {
            if (!m_shows[_idx])
            {
                Debug.Log("================ try to load");
                InstantiateAsync(_idx);
                m_shows[_idx] = true;
                //StartCoroutine(LoadTexture(_idx));
            }
        }
        else
        {
            if (m_shows[_idx])
            {
                if (m_objs[_idx])
                {
                    Debug.Log("================ try to release");
                    m_objs[_idx].SetActive(false);
                    //Material[] mats = m_objs[_idx].GetComponent<Renderer>().materials;
                    //foreach (Material mat in mats)
                    {
                        //Texture tex = mat.mainTexture;
                        //Resources.UnloadAsset(tex);
                    }
                    Addressables.ReleaseInstance(m_handles[_idx]);
                    //Destroy(m_objs[_idx]);
                    
                    Resources.UnloadUnusedAssets();
                    //Addressables.Release(m_handles[_idx]);
                    //GC.Collect();
                }

                m_shows[_idx] = false;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        //ShowHide(0);
        for(int i=0; i<m_objs.Length; i++)
        {
            ShowHide(i);
        }
    }
}
