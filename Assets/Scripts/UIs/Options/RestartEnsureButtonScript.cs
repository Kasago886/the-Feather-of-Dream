using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartEnsureButtonScript : MonoBehaviour
{
    SalManager salManager;
    // Start is called before the first frame update
    void Start()
    {
        salManager = FindAnyObjectByType<SalManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadCurrentArchive()
    {
        salManager.LoadCurrentArchive();
    }
}
