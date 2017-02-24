using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class M_TextureManageTest : MonoBehaviour
{
    string Path = "TestAtlas/123";
    string spriteName = "1P31Q002-0";
    Sprite _sprite;
    Image image;
    void Start()
    {
        //GetSpriteFormAtlas();
        EventTriggerListener.Get(gameObject).onClick = OnButtonClick;
    }


    private void OnButtonClick(GameObject go)
    {
        GetSpriteFormAtlas();
    }

    private void GetSpriteFormAtlas()
    {
        if (_sprite == null)
        {
            _sprite = M_TextureManage.Instance.LoadAtlasSprite(Path, spriteName);
        }
        if (image == null)
        {
            image = gameObject.GetComponent<Image>();
            image.sprite = _sprite;
            image.type = Image.Type.Filled;
            image.SetNativeSize();
        }
        image.fillAmount = 0.5f;
    }
    public void FillAmountCtrl()
    {
        image.fillAmount = 1.0f;
    }
}
