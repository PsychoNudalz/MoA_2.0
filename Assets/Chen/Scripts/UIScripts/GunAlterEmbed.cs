using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunAlterEmbed : MonoBehaviour
{
    [SerializeField] Button debugBtn;
    [SerializeField] GameObject debugMenu;
    [SerializeField] GameObject helpOverlay;
    [SerializeField] List<Sprite> sprites;
    private int currentSprite, spriteCount;

    void Start() {
        spriteCount = sprites.Count - 1;
    }
    public void DebugButtonOnClick() {
        debugMenu.SetActive(!debugMenu.activeSelf);
    }

    public void HelpButtonOnClick() {
        currentSprite = 0;
        helpOverlay.GetComponent<Image>().sprite = sprites[currentSprite];
        helpOverlay.SetActive(true);
    }

    public void HelpCloseOnClick() {
        helpOverlay.SetActive(false);
    }

    public void HelpNextOnClick() {
        if (currentSprite < spriteCount) {
            currentSprite ++;
            Debug.Log(currentSprite);
            helpOverlay.GetComponent<Image>().sprite = sprites[currentSprite];
        }
    }

    public void HelpPrevOnClick() {
        if (currentSprite > 0) {
            currentSprite --;
            Debug.Log(currentSprite);
            helpOverlay.GetComponent<Image>().sprite = sprites[currentSprite];
        }

    }
}
