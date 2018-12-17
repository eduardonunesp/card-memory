using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Numbers : MonoBehaviour
{
  public List<Sprite> numbers = new List<Sprite>();
  public uint Number
  {
    get { return _number; }
  }

  private SpriteRenderer _spriteRender;
  private uint _number;

  private void Awake()
  {
    _spriteRender = GetComponent<SpriteRenderer>();
    _spriteRender.sprite = numbers[1];
  }

  public void SetNumber(uint number)
  {
    _number = number;
    _spriteRender.sprite = numbers[(int)number];
  }
}
