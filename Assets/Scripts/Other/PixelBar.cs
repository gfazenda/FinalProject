using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PixelBar : MonoBehaviour {
    public Image barPrefab;
    bool isConfigured = false;
    public double minX, posY, maxX;

    public float ySize, offset;

    int currHP = 10, maxHP = 20;

    List<GameObject> hpBars = new List<GameObject>();
	// Use this for initialization
	void Start () {
        ySize /= 100;
        offset /= 100;
        UpdateBar(currHP,maxHP);
	}

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.X))
        {
            currHP -= 1;
            UpdateBar(5, maxHP);
        }
    }

    // Update is called once per frame
    public void UpdateBar(float current, float max)
    {
        if (!isConfigured)
        {
            Configure((int)max);
            return;
        }

        while(hpBars.Count > (int)current)
        {
            hpBars[hpBars.Count - 1].SetActive(false);
            hpBars.RemoveAt(hpBars.Count - 1);
        }

    }



    void Configure(int maxHP)
    {
        isConfigured = true;
        double availableSize = System.Math.Round((float)(maxX - minX) - ((maxHP-1) * offset),2);

        float currentX = (float)minX;
        double xSize = availableSize / maxHP;

        for (int i = 0; i < maxHP; i++)
        {
            Image newBar = Instantiate(barPrefab, this.transform);

            double finalX = (currentX + xSize);

            newBar.GetComponent<RectTransform>().anchorMin = new Vector2(currentX, (float)posY);
            newBar.GetComponent<RectTransform>().anchorMax = new Vector2((float)finalX, (float)(posY+ySize));

            newBar.gameObject.SetActive(true);

            currentX = (float)(finalX + offset);

            hpBars.Add(newBar.gameObject);
        }
    }
}
