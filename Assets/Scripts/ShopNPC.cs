using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(Dialogue))]
public class ShopNPC : MonoBehaviour
{
    [Header("Normal Dialogue")]
    [SerializeField] private string[] greetingLines;

    [Header("Shop Stock")]
    [SerializeField] private ItemPrice[] stock;

    [Header("Shop Messages")]
    [SerializeField] private string shopWelcomeMessage = "Welcome! Take a look at my wares.";
    [SerializeField] private string buyPrompt = "What would you like to buy?";
    [SerializeField] private string sellPrompt = "What would you like to sell?";
    [SerializeField] private string leaveMessage = "Come back soon!";

    private Dialogue dialogue;

    [Serializable]
    public class ItemPrice
    {
        public Item item;
        public int buyPrice = 50;
        public int sellPrice = 25;
    }

    private void Awake()
    {
        dialogue = GetComponent<Dialogue>();
    }

    public void StartShopDialogue()
    {
        var lines = new List<String>(greetingLines);

        lines.Add("");
        lines.Add("Would you like to buy something?");
        lines.Add("Yes/No");

    }


}
