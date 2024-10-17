using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocialRedirects : MonoBehaviour
{
    public void OpenSocialMediaLink(string url)
    {
        Application.OpenURL(url);
    }
}
