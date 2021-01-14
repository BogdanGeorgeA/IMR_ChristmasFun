using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SsAndShare : MonoBehaviour
{
	public GameObject layoutFiltre;
	public void ClickShare()
	{
		
		StartCoroutine(TakeScreenshotAndShare());
		
	}

	private IEnumerator TakeScreenshotAndShare()
	{
		yield return null;
		GameObject.Find("Canvas").GetComponent<Canvas>().enabled = false;
		yield return new WaitForEndOfFrame();
		Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
		ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
		ss.Apply();

		string timeStamp = System.DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss");
		string fileName = "Screenshot" + timeStamp + ".png";
		string filePath = System.IO.Path.Combine(Application.persistentDataPath, fileName);  //Application.temporaryCachePath
		System.IO.File.WriteAllBytes(filePath, ss.EncodeToPNG());

		// To avoid memory leaks
		Destroy(ss);

        GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
		// Share on WhatsApp only, if installed (Android only)
		//if( NativeShare.TargetExists( "com.whatsapp" ) )
		//	new NativeShare().AddFile( filePath ).AddTarget( "com.whatsapp" ).Share();
	}
}
