using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Storage;

public class Storage : MonoBehaviour
{
    [SerializeField] private Sprite spriteDummy;
    [SerializeField] private Texture2D texture;

    [SerializeField] private string path;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private bool upload = false;

    private FirebaseStorage _storageReference;
    private Sprite _newSprite;

    private void Start()
    {
        _storageReference = FirebaseStorage.DefaultInstance;

        if (upload)
        {
            StartCoroutine(UploadPicture(texture));
        }
        else
        {
            StartCoroutine(DownloadPicture(path));
        }

        
    }

    private IEnumerator UploadPicture(Texture2D picture)
    {
        //https://learn.microsoft.com/en-us/dotnet/api/system.guid?view=net-8.0
        var pictureReference = _storageReference.GetReference($"/pictures/{Guid.NewGuid()}.png");
        var bytes = picture.EncodeToPNG();
        var uploadTask = pictureReference.PutBytesAsync(bytes);

        yield return new WaitUntil(() => uploadTask.IsCompleted);

        if(uploadTask.Exception != null)
        {
            Debug.LogError($"Failed to upload because {uploadTask.Exception}");
            yield break;
        }

        var getUrlTask = pictureReference.GetDownloadUrlAsync();
        yield return new WaitUntil(() => getUrlTask.IsCompleted);

        if (getUrlTask.Exception != null)
        {
            Debug.LogError($"Failed to get download url with {getUrlTask.Exception}");
            yield break;
        }

        Debug.Log($"Download from {getUrlTask.Result}");
    }

    private IEnumerator DownloadPicture(string path)
    {
        var pictureReference = _storageReference.GetReference(path);

        var downloadTask = pictureReference.GetBytesAsync(long.MaxValue);

        yield return new WaitUntil(() => downloadTask.IsCompleted);

        if (downloadTask.Exception != null)
        {
            Debug.LogError($"Failed to download because {downloadTask.Exception}");
            yield break;
        }

        var texture = new Texture2D(2, 2);
        ImageConversion.LoadImage(texture, downloadTask.Result);

        //texture.LoadImage(downloadTask.Result);

        /*Texture2D blankTexture = new Texture2D(1024, 1024, TextureFormat.RGBA32, true);
        blankTexture.LoadImage(downloadTask.Result);*/

        Sprite blankSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        
        spriteRenderer.sprite = blankSprite;
    }
}
