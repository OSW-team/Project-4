using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class ModuleButtonBehaviour : MonoBehaviour
{
    public GameObject ModuleButtonGO;
    public List<Sprite> ShowModuleSprites;
    public List<GameObject> Subsystems; 
    public bool Expanded;
    private bool _isAnimating;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OnCick()
    {
        if (!_isAnimating)
        {
            Expanded = !Expanded;
            AnimateModuleExpand(Expanded);
        }

        
        
    }

    private void AnimateModuleExpand(bool expand)
    {
        StartCoroutine(AnimateModuleExpandCoroutine(expand));
    }

    private IEnumerator AnimateModuleExpandCoroutine(bool expand)
    {
        _isAnimating = true;
        var sprites = new List<Sprite>(ShowModuleSprites);
        if (!expand)
        {
            sprites.Reverse();
            foreach (var subsystem in Subsystems)
            {
                subsystem.SetActive(false);
            }
        }
        var moduleImage = ModuleButtonGO.GetComponent<Button>();
        foreach (var sprite in sprites)
        {
            moduleImage.image.sprite = sprite;
            moduleImage.image.rectTransform.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height);
            yield return new WaitForSeconds(0.03f);
        }
        if (expand)
        {
            sprites.Reverse();
            foreach (var subsystem in Subsystems)
            {
                subsystem.SetActive(true);
            }
        }
        _isAnimating = false;
    }
}
