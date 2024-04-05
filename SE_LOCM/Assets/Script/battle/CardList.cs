using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;
using System;

public class CardList
{
    public List<Card> Cards { get; private set; }
    public int Count { get { return Cards.Count; } }
    public int CountAttackCards => Cards.Count(card => card.type == CardType.Attack);
    public int CountSpellCards => Cards.Count(card => card.type == CardType.Spell);
    public int CountEquipCards => Cards.Count(card => card.type == CardType.Equip);
    public int CountQuizCards => Cards.Count(card => card.type == CardType.Quiz);

    public CardList()
    {
        Cards = new();
    }
    public CardList(List<Card> cards)
    {
        Cards = cards;
    }
    public virtual void AddCard(Card card)
    {
        if (card is null) return;
        Cards.Add(card);
    }
    public virtual void AddCards(List<Card> cards)
    {
        foreach (var card in cards) AddCard(card);
    }
    public virtual void RemoveCard(Card card)
    {
        Cards.Remove(card);
    }
    public virtual void RemoveCards(List<Card> cards)
    {
        foreach (var card in cards) RemoveCard(card);
    }
    public bool ContainsCard(Card card)
    {
        return Cards.Contains(card);
    }
    public void ClearCards()
    {
        Cards.Clear();
    }
    public void UpgradeAllCards()
    {
        foreach (var card in Cards) card.Upgrade();
    }
    public List<Card> DisposeNonAttackCards()
    {
        List<Card> waitDispose = new();
        foreach (var card in Cards)
        {
            if (card.type != CardType.Attack) waitDispose.Add(card);
        }
        foreach (var card in waitDispose)
        {
            Cards.Remove(card);
        }
        return waitDispose;
    }
    public Card Top()
    { return Cards[^1]; }
}

public class CardPile : MonoBehaviour
{
    protected CardList cardList;
    public List<Card> Cards { get { return cardList.Cards; } }
    public int Count { get { return Cards.Count; } }
    public int CountAttackCards { get { return cardList.CountAttackCards; } }
    public int CountSpellCards { get { return cardList.CountSpellCards; } }
    public int CountEquipCards { get { return cardList.CountEquipCards; } }
    public int CountQuizCards { get { return cardList.CountQuizCards; } }
    public void AddCard(Card card) { cardList.AddCard(card); }
    public virtual void AddCards(List<Card> cards) { cardList.AddCards(cards); }
    public void RemoveCard(Card card) { cardList.RemoveCard(card); }
    public virtual void RemoveCards(List<Card> cards) { cardList.RemoveCards(cards); }
    public bool ContainsCard(Card card) { return cardList.ContainsCard(card); }
    public void ClearCards() { cardList.ClearCards(); }
    public void UpgradeAllCards() { cardList.UpgradeAllCards(); }
    public Card Top() { return Cards[^1]; }
    private void Start()
    {
        cardList = new();
    }
}

public class RoundPlayedCards : CardList
{
    private readonly GameController gc;
    public RoundPlayedCards(GameController gc)
    {
        this.gc = gc;
    }
    public override void AddCard(Card card)
    {
        base.AddCard(card);
        if (card.type == CardType.Attack && CountAttackCards % 3 == 0)
        {
            if (gc.player.ContainsBook(31)) gc.player.ap++;
            if (gc.player.ContainsBook(33)) gc.AllAttack(5);
        }
        if (card.type == CardType.Spell && CountSpellCards % 3 == 0)
        {
            if (gc.player.ContainsBook(32)) gc.player.dp++;
            if (gc.player.ContainsBook(34)) gc.AddShield(5);
        }
    }
}