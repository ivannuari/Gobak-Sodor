using System;
using UnityEngine;
using System.Threading;

public class CharacterHandler : MonoBehaviour
{
    public bool isPlayer = true;
    public CharacterType characterType = CharacterType.Penonton;
    public CharacterMode characterMode = CharacterMode.InMenu;

    public SkinnedMeshRenderer clothRenderer;
    public SkinnedMeshRenderer hairRenderer;

    public int characterIndex;

    private Animator anim;

    private async void Start()
    {
        anim = GetComponentInChildren<Animator>();

        SetCharacter();
        GameBehaviour.Instance.OnJerseyColorChanged += ChangeSuitColor;

        await Awaitable.WaitForSecondsAsync(0.25f, CancellationToken.None);

        ChangeHairColor();
        ChangeSuitColor(GameBehaviour.Instance.characterJerseyColor);
    }

    private void ChangeHairColor()
    {
        Color randColor = UnityEngine.Random.ColorHSV();
        hairRenderer.material.SetColor("_HAIRCOLOR", randColor);
    }

    private void OnDestroy()
    {
        GameBehaviour.Instance.OnJerseyColorChanged -= ChangeSuitColor;
    }

    private void SetCharacter()
    {
        switch (characterType)
        {
            case CharacterType.Penyerang:
                switch (characterMode)
                {
                    case CharacterMode.InPlay:
                        anim.Play("Penyerang");
                        break;
                    case CharacterMode.InMenu:
                        SetMenuPose();
                        break;
                }
                break;
            case CharacterType.Penjaga:
                anim.Play("Penjaga");
                break;
            case CharacterType.Penonton:
                SetPenontonPose();
                break;
        }
    }

    private void SetPenontonPose()
    {
        anim.Play($"Penonton {characterIndex}");
        Material mat = clothRenderer.material;

        Color randColor = UnityEngine.Random.ColorHSV();
        mat.SetColor("_CLOTH3COLOR", randColor);
    }

    private void SetMenuPose()
    {
        anim.Play($"Pose {characterIndex}");
    }

    public void ChangeSuitColor(Color newColor)
    {
        if (!isPlayer) { return; }

        Material mat = clothRenderer.material;

        // Ubah CLOTH 1 COLOR
        mat.SetColor("_CLOTH3COLOR", newColor);
    }
}

public enum CharacterType
{
    Penyerang,
    Penjaga,
    Penonton
}

public enum CharacterMode
{
    InPlay,
    InMenu
}
