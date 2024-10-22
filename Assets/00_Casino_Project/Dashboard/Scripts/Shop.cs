using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.IO;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    public static Shop Inst;
    public GameObject GreenSelection, Deposit_BTN, Support_BTN, ManualPayment_BTN;
    public GameObject Deposit_Box, Support_Box, ManualPayment_Box;
    public GameObject MP_QR_Box, MP_BANK_Box, MP_UPI_ID_Box,MP_CRYPTO_QR, MP_CRYPTO_ID;
    public Text Header_Text_MSG, TxtShop_Balanch;
    public Text TxtPrinchipal, TxtBonus,TxtNewAmount;
    public Text Txt_Support_UserID;
    public PFB_SHOP _PFB_SHOP;
    public PFB_SUPPORT _PFB_SUPPORT;
    public RectTransform DataParent, DataParent_SUPPORT;
    public List<GameObject> CellList;
    public Transform Select_Upi_header;
    public Transform Select_PAYMENT_header;
    [SerializeField] List<Text> LBL_UPI_LIST;
    public string selected_ID, selected_Plan;
    [SerializeField]public InputField InputRefralCode;

    [Header(":-- Manual Payment --:")]
    [SerializeField] Text TXT_MP_AC_NO;
    [SerializeField] Text TXT_MP_IFSC;
    [SerializeField] Text TXT_MP_USER_NAME;
    [SerializeField] Text TXT_MP_BANK_NAME;
    [SerializeField] Text TXT_UPI_ID;
    [SerializeField] IMGLoader IMG_QR;
    [SerializeField] Text TXT_CRYPTO_ID;
    [SerializeField] IMGLoader IMG_CRYPTO_QR;
    [SerializeField] List<Text> LBL_PAYMENT_TITLE_LIST;

    internal Permission _permission;
    private byte[] Icondata;

    internal string FilePath,Uploaded_URL;
    internal string commpressFilePath;

    public Text Txt_Withdraw_Amount,TxtPayAmount;
    public string Final_PayAmount;
    [SerializeField] Text Selected_SS_Path;

    public GameObject RefCode_Lable_OBJ, RefCode_Input_OBJ;

    // Start is called before the first frame update
    void Start()
    {
        Inst = this;

        if (GS.Inst.low_balance_warning && SceneManager.GetActiveScene().name == "Dashboard")
        {
           OPEN_SHOP();
           GS.Inst.low_balance_warning = false;
        }
    }

    public void SET_DEPOSIT_DATA(JSONObject data)
    {
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_SHOP();
        string referalcode=data.GetField("refferal_code").ToString().Trim(Config.Inst.trim_char_arry);
        InputRefralCode.text = referalcode;

        if (referalcode!="")
        {
            RefCode_Lable_OBJ.transform.localScale = Vector3.zero;
            RefCode_Input_OBJ.transform.localScale = Vector3.zero;
        }
        else
        {
            RefCode_Lable_OBJ.transform.localScale = Vector3.one;
            RefCode_Input_OBJ.transform.localScale = Vector3.one;
        }

        selected_ID = data.GetField("store_details").GetField("_id").ToString().Trim(Config.Inst.trim_char_arry);
        for (int i = 0; i < data.GetField("titles").Count; i++)
        {
            LBL_UPI_LIST[i].text= data.GetField("titles")[i].ToString().Trim(Config.Inst.trim_char_arry);
        }
        for (int i = 0; i < data.GetField("store_details").GetField("stores").Count; i++)
        {
            PFB_SHOP cell = Instantiate(_PFB_SHOP, DataParent) as PFB_SHOP;
            CellList.Add(cell.gameObject);
            cell.SET_DATA(data.GetField("store_details").GetField("stores")[i],i);
        }
        DataParent.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }
    public void SET_SUPPORT_DATA(JSONObject data)
    {
        DataParent_SUPPORT.parent.parent.GetComponent<ScrollRect>().enabled = false;
        Clear_OLD_SHOP();
        //PFB_SUPPORT cell = Instantiate(_PFB_SUPPORT, DataParent_SUPPORT) as PFB_SUPPORT;
        //CellList.Add(cell.gameObject);
        //cell.First_DATA();

        //PFB_SUPPORT cell2 = Instantiate(_PFB_SUPPORT, DataParent_SUPPORT) as PFB_SUPPORT;
        //CellList.Add(cell2.gameObject);
        //cell2.First_DATA2();
        for (int i = 0; i < data.GetField("support_lists").Count; i++)
        {
            PFB_SUPPORT cell = Instantiate(_PFB_SUPPORT, DataParent_SUPPORT) as PFB_SUPPORT;
            CellList.Add(cell.gameObject);
            cell.SET_DATA(data.GetField("support_lists")[i]);
        }
        DataParent_SUPPORT.parent.parent.GetComponent<ScrollRect>().enabled = true;
    }
    public void Clear_OLD_SHOP()
    {
        Comen_Event_Setup.Inst.Clear_Shop_PFB();
        if (CellList.Count > 0)
        {
            for (int i = 0; i < CellList.Count; i++)
            {
                if (CellList[i] != null)
                    Destroy(CellList[i]);
            }
            CellList.Clear();
        }
    }

    void RESET_DATA_AFTER_SUBMIT()
    {
        TxtPayAmount.text = "";
        Final_PayAmount="";
        Selected_SS_Path.text = "Select Image";
        Txt_Withdraw_Amount.text = "";
        FilePath = "";
    }

    public void SET_MANUAL_PAYMENT_DATA(JSONObject data)
    {
        TXT_MP_AC_NO.text = data.GetField("bank_info").GetField("account_number").ToString().Trim(Config.Inst.trim_char_arry);
        TXT_MP_BANK_NAME.text = data.GetField("bank_info").GetField("bank_name").ToString().Trim(Config.Inst.trim_char_arry);
        TXT_MP_IFSC.text = data.GetField("bank_info").GetField("ifsc_code").ToString().Trim(Config.Inst.trim_char_arry);
        TXT_MP_USER_NAME.text = data.GetField("bank_info").GetField("user_name").ToString().Trim(Config.Inst.trim_char_arry);

        TXT_UPI_ID.text = data.GetField("upi_id").ToString().Trim(Config.Inst.trim_char_arry);
        IMG_QR.LoadIMG(data.GetField("qr_code_url").ToString().Trim(Config.Inst.trim_char_arry),false, true);

        TXT_CRYPTO_ID.text = data.GetField("crypto_id").ToString().Trim(Config.Inst.trim_char_arry);
        IMG_CRYPTO_QR.LoadIMG(data.GetField("crypto_qr_code_url").ToString().Trim(Config.Inst.trim_char_arry),false, true);

        //set header
        for (int i = 0; i < data.GetField("titles").Count; i++)
        {
            LBL_PAYMENT_TITLE_LIST[i].text = data.GetField("titles")[i].ToString().Trim(Config.Inst.trim_char_arry);
        }
    }

    public void OPEN_SHOP()
    {
        SoundManager.Inst.PlaySFX(0);
        Selected_SS_Path.text = "Select Image";
        Txt_Withdraw_Amount.text = "";
        FilePath = "";
        TxtShop_Balanch.text = GS.Inst._userData.Chips.ToString("n2");
        GS.Inst.iTwin_Open(this.gameObject);
        OPEN_DEPOSIT();
    }
    public void CLOSE_SHOP()
    {
        SoundManager.Inst.PlaySFX(0);
        GS.Inst.iTwin_Close(this.gameObject, 0.3f);
    }

    public void OPEN_DEPOSIT()
    {
        SoundManager.Inst.PlaySFX(0);
        Header_Text_MSG.text = "Please select the amount! कृपया राशि का चयन करें!";
        Deposit_Box.transform.localScale = Vector3.one;
        Support_Box.transform.localScale = Vector3.zero;
        Selected_SS_Path.text = "Select Image";
        ManualPayment_Box.transform.localScale = Vector3.zero;
        GreenSelection.transform.localPosition = Deposit_BTN.transform.localPosition;
        Select_Upi_header.transform.localPosition = GameObject.Find("BTN_UPI-11").transform.localPosition;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.DEPOSIT_LISTS(""));
    }
    public void OPEN_SUPPORT()
    {
        SoundManager.Inst.PlaySFX(0);
        Header_Text_MSG.text = "If you have anything unsure, please contact the customer support.   यदि आपके पास कोई अन्य समस्या है, तो कृपया ग्राहक सहायता से संपर्क करें!";
        Txt_Support_UserID.text = GS.Inst._userData.UID;
        Support_Box.transform.localScale = Vector3.one;
        Deposit_Box.transform.localScale = Vector3.zero;
        ManualPayment_Box.transform.localScale = Vector3.zero;
        GreenSelection.transform.position=Support_BTN.transform.position;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SUPPORTS());
    }

    public void OPEN_MANUAL_PAYMENT()
    {
        SoundManager.Inst.PlaySFX(0);

        Header_Text_MSG.text = "If you have anything unsure, please contact the customer support.   यदि आपके पास कोई अन्य समस्या है, तो कृपया ग्राहक सहायता से संपर्क करें!";
        Txt_Support_UserID.text = GS.Inst._userData.UID;
        ManualPayment_Box.transform.localScale = Vector3.one;
        Support_Box.transform.localScale = Vector3.zero;
        Deposit_Box.transform.localScale = Vector3.zero;
        UPI_HEADER_CLICK_PAYMENT(GameObject.Find("BTN_UPI_QR").gameObject);
        //GreenSelection.transform.position = ManualPayment_BTN.transform.position;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.MANUAL_PAYMENT_INFO());
    }

    public void UPI_HEADER_CLICK_PAYMENT(GameObject Upi_OBJ)
    {
        SoundManager.Inst.PlaySFX(0);
        MP_QR_Box.transform.localScale = Vector3.zero;
        MP_BANK_Box.transform.localScale = Vector3.zero;
        MP_UPI_ID_Box.transform.localScale = Vector3.zero;
        MP_CRYPTO_QR.transform.localScale = Vector3.zero;
        MP_CRYPTO_ID.transform.localScale = Vector3.zero;
        Select_PAYMENT_header.transform.position = Upi_OBJ.transform.position;

        if (Upi_OBJ.name.Equals("BTN_UPI_QR"))
            MP_QR_Box.transform.localScale = Vector3.one;
        else if (Upi_OBJ.name.Equals("BTN_BANK_DETAILS"))
            MP_BANK_Box.transform.localScale = Vector3.one;
        else if (Upi_OBJ.name.Equals("BTN_UPI_ID"))
        {
            UniClipboard.SetText(TXT_UPI_ID.text);
            MP_UPI_ID_Box.transform.localScale = Vector3.one;
        }
        else if (Upi_OBJ.name.Equals("BTN_Crypto_QR"))
            MP_CRYPTO_QR.transform.localScale = Vector3.one;
        else if (Upi_OBJ.name.Equals("BTN_Crypto_ID"))
        {
            UniClipboard.SetText(TXT_CRYPTO_ID.text);
            MP_CRYPTO_ID.transform.localScale = Vector3.one;
        }
    }
    public void BTN_COPY()
    {
        UniClipboard.GetText();
    }
    public void BTN_ADD_CHIPS()
    {
        SoundManager.Inst.PlaySFX(0);
        SocketHandler.Inst.SendData(SocketEventManager.Inst.SUCCESS_DEPOSIT(selected_ID,selected_Plan, InputRefralCode.text));
    }

    public void UPI_HEADER_CLICK(GameObject Upi_OBJ)
    {
        SoundManager.Inst.PlaySFX(0);
        Select_Upi_header.transform.position = Upi_OBJ.transform.position;
        SocketHandler.Inst.SendData(SocketEventManager.Inst.DEPOSIT_LISTS(Upi_OBJ.transform.GetChild(0).GetComponent<Text>().text));
    }

    public void BTN_Copy_Support_ID()
    {
        SoundManager.Inst.PlaySFX(0);
        UniClipboard.SetText(GS.Inst._userData.UID);
        GameObject.Find("Cop_Alert").transform.localScale = Vector3.one;
        UniClipboard.GetText();
        Invoke("CL_ALT", 0.5f);
    }
    void CL_ALT()
    {
        GameObject.Find("Cop_Alert").transform.localScale = Vector3.zero;
    }

    public void Add_Chips_Success(JSONObject data)
    {
        CLOSE_SHOP();
        Alert_MSG.Inst.MSG("Deposit successful with amount Rs."+data.GetField("stores")[0].GetField("amount").ToString().Trim(Config.Inst.trim_char_arry)+" with extra bonus "+data.GetField("stores")[0].GetField("extra_per").ToString().Trim(Config.Inst.trim_char_arry));
    }

    public void OpenGallery()
    {
        SoundManager.Inst.PlaySFX(0);
        if (NativeGallery.IsMediaPickerBusy())
            return;
        PickImage(512);
    }

    internal void PickImage(int maxSize)
    {
        if (NativeGallery.CheckPermission(NativeGallery.PermissionType.Read,NativeGallery.MediaType.Image) == NativeGallery.Permission.Denied)
        {
            Alert_MSG.Inst.MSG("Please allow access to the gallery");
            return;
        }

        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Out " + path);

            if (path != null)
            {
                Debug.Log("In " + path);
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize, false);
                if (texture == null)
                {
                    Debug.Log("Couldn't load texture from " + path);
                    return;
                }

                string fileName = path.Split('/')[path.Split('/').Length - 1];
                //   Debug.LogError(fileName + " : " + path);
                if (fileName.Contains(".png") || fileName.Contains(".PNG"))
                {
                    System.IO.File.WriteAllBytes(Application.temporaryCachePath + "/Pickedimage.png",
                        texture.EncodeToPNG());
                    FilePath = Application.temporaryCachePath + "/Pickedimage.png";
                }
                else
                {
                    System.IO.File.WriteAllBytes(Application.temporaryCachePath + "/Pickedimage.jpg",
                        texture.EncodeToJPG());
                    FilePath = Application.temporaryCachePath + "/Pickedimage.jpg";
                }

                texture.name = path.Split('/')[path.Split('/').Length - 1];
                Selected_SS_Path.text = FilePath;
            }
            else
            {
                //Classes.instance.uploadImageModuleRes = null;
            }
        });
        Debug.Log("Permission result: " + permission);
    }

    public void Upload_SS()
    {
        SoundManager.Inst.PlaySFX(0);
        if (Txt_Withdraw_Amount.text != "" && Txt_Withdraw_Amount.text != " " && FilePath!="")
        {
            PreeLoader.Inst.Show();
            StartCoroutine(UploadToAPI());
        }
        else
        {
            Alert_MSG.Inst.MSG("Please select payment screenshot first.");
        }
    }

    IEnumerator UploadToAPI()
    {
        //ImageCompress.Inst.performCompressAction(FilePath);
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(UploadDetailseAPI());
    }

    byte[] imageToSend;
    public IEnumerator UploadDetailseAPI()
    {
        yield return new WaitForSeconds(2f);
        string panDetailURL = Config.Inst.BaseURL + "paymentSSUplolad";
        Debug.Log("URL ::: > " + panDetailURL);

        byte[] data = File.ReadAllBytes(FilePath);
        Texture2D tex = new Texture2D(8, 8);
        tex.LoadImage(data);
        tex.Apply();
        imageToSend = tex.EncodeToPNG();

        WWWForm form = new WWWForm();
        form.headers["enctype"] = "multipart/form-data";
        form.AddBinaryData("photos", imageToSend, "screenShot.png", "image/png");
        //form.AddField("id", GS.Inst._userData.Id);
        //form.AddField("ss_url", panDetailURL);
        //form.AddField("amount", Input_Withdraw_Amount.text);

        using (UnityWebRequest www = UnityWebRequest.Post(panDetailURL, form))
        {
            www.method = "POST";
            yield return www.SendWebRequest();
            //PreeLoader.Inst.Stop_Loader();
            //CancelInvoke("Close_LOADER");
            if (www.isNetworkError || www.isHttpError)
            {
                PreeLoader.Inst.Stop_Loader();
                Debug.Log(www.error);
                Alert_MSG.Inst.MSG("Server error please try again!");
            }
            else
            {
                PreeLoader.Inst.Stop_Loader();
                Alert_MSG.Inst.MSG("Screenshot uploaded successfully.");
                Debug.Log(" upload complete!" + www.downloadHandler.text);
                JSONObject data_JSON = new JSONObject(www.downloadHandler.text);
                Uploaded_URL = data_JSON.GetField("image").ToString().Trim(new char[] { '"' });
                SocketHandler.Inst.SendData(SocketEventManager.Inst.MANUAL_PAYMENT(Uploaded_URL, Final_PayAmount));
                RESET_DATA_AFTER_SUBMIT();

                if (SceneManager.GetActiveScene().name == "Dashboard")
                {
                    if (VIP.Inst.transform.localScale.x > 0)
                        VIP.Inst.OPEN_VIP();
                }

                if (SceneManager.GetActiveScene().name != "Dashboard" && SceneManager.GetActiveScene().name != "Splace" && SceneManager.GetActiveScene().name != "Login")
                    SocketHandler.Inst.Reload_Game();
            }
        }
    }
}
