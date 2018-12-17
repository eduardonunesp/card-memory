using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Memory
{
  public class GameManager : MonoBehaviour
  {
    public static GameManager Instance { get { return _instance; } }
    public uint totalCards = 9;
    public GameObject card;
    public AudioClip right;
    public AudioClip wrong;
    public bool CanFlip
    {
      get
      {
        return _cardsFlipped.Count < _numberOfPairs;
      }
    }
    public bool AnyFlipped { get { return _cardsFlipped.Count > 0; } }

    private static GameManager _instance;
    private AudioSource _audioSource;
    private List<Card> _cards = new List<Card>();
    private List<Card> _cardsFlipped = new List<Card>();
    private Transform _startPoint;
    private uint _numberOfPairs = 2;
    private float _timerHide;
    private float _timerHideLimit = 2;

    private void Awake()
    {
      if (_instance == null)
        _instance = this;
      else if (_instance != this)
        Destroy(gameObject);

      for (uint i = 0; i < totalCards; i++)
      {
        for (uint j = 0; j < 2; j++)
        {
          GameObject goCard = Instantiate(card, transform.position, Quaternion.identity);
          Card newCard = goCard.GetComponent<Card>();
          newCard.SetNumber(i);
          _cards.Add(newCard);
        }
      }

      for (int i = 0; i < _cards.Count; i++)
      {
        Card temp = _cards[i];
        int randomIndex = Random.Range(i, _cards.Count);
        _cards[i] = _cards[randomIndex];
        _cards[randomIndex] = temp;
      }
    }

    private void Start()
    {
      _audioSource = GetComponent<AudioSource>();
      _timerHide = _timerHideLimit;
      _startPoint = GameObject.Find("StartPoint").gameObject.GetComponent<Transform>();
      InstantiateCards();
    }

    private void Update()
    {
      TimerToHideNonMatchingCards();
      CheckWon();
    }

    private void TimerToHideNonMatchingCards()
    {
      if (CanFlip) return;
      if (!AnyFlipped) return;
      _timerHide -= Time.deltaTime;
      if (_timerHide <= 0)
      {
        HideNonMatchingCards();
      }
    }

    private bool HasAnyCardFaceUp()
    {
      return _cards.Exists(card => card.IsFaceUp);
    }

    private bool AllCardsFound()
    {
      return _cards.TrueForAll(card => card.IsFound);
    }

    private void HideNonMatchingCards()
    {
      foreach (var cardToHide in _cards)
      {
        if (!cardToHide.IsFound && cardToHide.IsFaceUp)
        {
          cardToHide.Hide();

          CleanUpCardsFlipped();

          Debug.Log("Hides card: " + cardToHide.GetInstanceID());
        }

        _timerHide = _timerHideLimit;
      }
    }

    private void CleanUpCardsFlipped()
    {
      _cardsFlipped.Clear();
    }

    private void InstantiateCards()
    {
      int x = 0;
      int y = 0;
      for (int i = 0; i < _cards.Count; i++)
      {
        if (i < 6)
        {
          x = i;
          y = 0;
        }

        if (i > 5)
        {
          x = i - 6;
          y = 1;
        }

        if (i > 11)
        {
          x = i - 12;
          y = 2;
        }

        SpriteRenderer spriteRenderer = card.transform.Find("CardBack").gameObject.GetComponent<SpriteRenderer>();
        Vector3 newPosition = new Vector3(
          _startPoint.transform.position.x + (x * (spriteRenderer.bounds.size.x + 0.2f)),
          _startPoint.transform.position.y - (y * (spriteRenderer.bounds.size.y + 0.2f)),
          0);
        Card newCardPos = _cards[i];
        newCardPos.transform.position = newPosition;
      }
    }

    public void CardFlip(Card card)
    {
      _cardsFlipped.Add(card);
      Debug.Log("Card Flipped" + _cardsFlipped.Count);

      if (_cardsFlipped.Count >= _numberOfPairs)
      {
        if (_cardsFlipped[0].Compare(_cardsFlipped[1]))
        {
          foreach (var cardFlipped in _cardsFlipped)
          {
            cardFlipped.SetFound();
          }

          CleanUpCardsFlipped();

          _audioSource.clip = right;
          _audioSource.Play();
        }
        else
        {
          _audioSource.clip = wrong;
          _audioSource.Play();
        }
      }
    }

    private void CheckWon()
    {
      if (AllCardsFound())
      {
        StartCoroutine(EndScene(2));
      }
    }

    private IEnumerator EndScene(int counter)
    {
      while (counter > 0)
      {
        yield return new WaitForSeconds(1);
        counter--;
      }
      SceneManager.LoadScene("GameScene");
    }
  }
}