﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class GameController : MonoBehaviourPun, IPunTurnManagerCallbacks
{
    public GameObject[] Pieces;
    [SerializeField]
    public bool[] fullDeck;
    string thisPieceName;
    public int amountOfCards = 28;
    public ServerData serverData;

    
    //PlayerController playerController;

    //private GameObject FirstPiece;

    private GameObject Table;
    private Transform playerHand;

    
    
    private void Start()
    {
        amountOfCards = 28;
        fullDeck = new bool[28];
        ResetCards(fullDeck);

        playerHand = GameObject.FindGameObjectWithTag("PlayerHand").transform;
        serverData = this.GetComponent<ServerData>();
        //playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        Table = GameObject.FindGameObjectWithTag("Table");

        if (Table == null)
            Debug.LogError("There's no table");

        /*if(PhotonNetwork.IsMasterClient)
        {
            DistributeCards2(PhotonNetwork.PlayerList.Length);
        }*/

        /*if(PhotonNetwork.IsMasterClient)
        {
            InstantiateFirstCard();
        }
        */

        AddPlayerOnList();

        serverData.PrintAllPlayers();

        if (PhotonNetwork.IsMasterClient)
            serverData.Distribute();

        serverData.SelectBiggestBomb();
        OrganizePlayerList();

        serverData.PrintAllPlayers();
    }

    public void OrganizePlayerList()
    {
        serverData.OrganizePlayerListpt1();
        serverData.OrganizePlayerListpt2();

    }
    public void AddPlayerOnList()
    {
        serverData.AddPlayer(PhotonNetwork.NickName);
        //serverData.AddPlayerPC(playerController);
    }


    public void InstantiateFirstCard()
    {
        int i = Random.Range(0, 27);
        Debug.Log("Numero sorteado: " + i);
        thisPieceName = Pieces[i].name;

        serverData.SetPieceOn(thisPieceName, new Vector3(0,0,0), new Quaternion(0,0,0,0), true);
        amountOfCards--;
        serverData.RefreshAmountOfCards(amountOfCards);
        serverData.SavePickedPieces(i, true);
    }



    public void TurnPieceVisible()
    {
        if (amountOfCards == 0)
            return;

        /*int j = Random.Range(0, 10000)%28;
        while (fullDeck[j] == true)
        {
            j = Random.Range(0, 10000)%28;
        }*/

        int j = serverData.AddOnListOfCards();

        Debug.Log("Numero sorteado: " + j);

        if (fullDeck == null)
            Debug.LogError("fullDeck deu ruim");

        GameObject thisPiece = Pieces[j];

        if (thisPiece == null)
            Debug.LogError("thisPiece deu ruim");

        thisPiece.transform.SetParent(playerHand, true);
        thisPiece.transform.position = new Vector3(0, 0, 0);
        thisPiece.transform.rotation = new Quaternion(0, 0, 0, 0);

        if(Pieces[j].GetComponent<DraggablePiece>().isDouble && Pieces[j].GetComponent<DraggablePiece>().cardNumber > serverData.biggestBomb)
            serverData.biggestBomb = Pieces[j].GetComponentInChildren<PieceBehaviour>().value;


        amountOfCards--;
        serverData.RefreshAmountOfCards(amountOfCards);
        serverData.SavePickedPieces(j, true);
    }

    /*


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(fullDeck);
        }
        else if (stream.IsReading)
        {
            SetDeck((bool[])stream.ReceiveNext());
        }
    }

    private void SetDeck(bool[] thisDeck)
    {
        for (int i = 0; i < thisDeck.Length; i++)
        {
            if (thisDeck[i] == fullDeck[i])
                return;

            fullDeck[i] = thisDeck[i];
        }
    }
    */

    private void ResetCards(bool[] deck)
    {
        for (int i = 0; i < deck.Length; i++)
        {
            deck[i] = false;
        }
    }

    void IPunTurnManagerCallbacks.OnTurnBegins(int turn)
    {

    }

    void IPunTurnManagerCallbacks.OnTurnCompleted(int turn)
    {

    }

    void IPunTurnManagerCallbacks.OnPlayerMove(Player player, int turn, object move)
    {

    }

    void IPunTurnManagerCallbacks.OnPlayerFinished(Player player, int turn, object move)
    {

    }

    void IPunTurnManagerCallbacks.OnTurnTimeEnds(int turn)
    {

    }
}
