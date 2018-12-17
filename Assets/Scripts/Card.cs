using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Memory
{
  public class Card : MonoBehaviour
  {
    public GameObject cardBack;
    public GameObject cardFront;
    public GameObject cardNumber;
    public uint Number { get { return _number; } }
    public bool IsFound { get { return _found; } }
    public bool IsFaceUp { get { return !_cardBackSpriteRenderer.enabled; } }

    private bool _found = false;
    private uint _number = 0;
    private SpriteRenderer _cardBackSpriteRenderer;

    private void Start()
    {
      _cardBackSpriteRenderer = cardBack.GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
      if (!GameManager.Instance.CanFlip) return;
      Show();
    }

    public void SetFound()
    {
      _found = true;
    }

    public void SetNumber(uint number)
    {
      _number = number;
      cardNumber.GetComponent<Numbers>().SetNumber(_number);
    }

    public void Hide()
    {
      if (_found) return;
      _cardBackSpriteRenderer.enabled = true;
      Debug.Log("Hides card face: " + GetInstanceID());
    }

    public void Show()
    {
      if (_found) return;
      GameManager.Instance.CardFlip(this);
      _cardBackSpriteRenderer.enabled = false;
      Debug.Log("Shows card face: " + GetInstanceID());
    }

    public bool Compare(Card card)
    {
      return this.Number == card.Number;
    }
  }
}
