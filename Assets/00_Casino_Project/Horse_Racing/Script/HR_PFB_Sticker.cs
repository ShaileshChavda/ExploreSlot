using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HR_PFB_Sticker : MonoBehaviour
{
    public static HR_PFB_Sticker Inst;
    public Image myImage;
    [SerializeField] Animator MyAnim;
    string Anim_Name;
    // Start is called before the first frame update
    void Start()
    {
        Inst = this;
    }
    public void SET_Sticker(int stickerName)
    {
        myImage.sprite = HR_Chat.Inst.Emoji_sprite_List[stickerName-1];
        Anim_Name = stickerName.ToString();
        Invoke(nameof(Start_StickerAnim),1f);
    }
    public void Start_StickerAnim()
    {
        MyAnim.enabled = true;
        MyAnim.Play(Anim_Name);
        Invoke(nameof(Kill),2f);
    }
    void Kill()
    {
        Destroy(this.gameObject,0f);
    }
}
