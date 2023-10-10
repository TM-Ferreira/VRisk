using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SetNewMaterial : MonoBehaviour
{
    public MeshRenderer meshRend;
    public GameObject pavement;

    public enum MaterialSelection : int
    {
        CYAN,
        GREEN,
        GREY,
        PINK,
        WHITE,
        YELLOW
    }

    private bool pavementEnabled = true;
    private MaterialSelection selectedMaterial;
    [SerializeField] private Material cyanMat;
    [SerializeField] private Material greenMat;
    [SerializeField] private Material greyMat;
    [SerializeField] private Material pinkMat;
    [SerializeField] private Material whiteMat;
    [SerializeField] private Material yellowMat;

    public void setPavement(bool visible)
    {
        pavementEnabled = visible;
    }

    public bool getPavement()
    {
        return pavementEnabled;
    }

    public void setMat(MaterialSelection selection)
    {
        selectedMaterial = selection;
    }

    public MaterialSelection getMat()
    {
        return selectedMaterial;
    }

    //This is horrible. I hate it
    public void ChangeMaterial()
    {
        meshRend.material = selectedMaterial switch
        {
            MaterialSelection.CYAN => cyanMat,
            MaterialSelection.GREEN => greenMat,
            MaterialSelection.GREY => greyMat,
            MaterialSelection.PINK => pinkMat,
            MaterialSelection.WHITE => whiteMat,
            MaterialSelection.YELLOW => yellowMat,
            _ => meshRend.material
        };
        
        pavement.SetActive(pavementEnabled);
    }
}
