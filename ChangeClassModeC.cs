using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeClassModeC : ChangeClassMode
{

    // Update is called once per frame
    protected override void Update()
    {
        if (Input.GetKeyDown("p")) Debug.Log("C");
    }
}
