﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
  #region Public Fields
  [SerializeField]
  [Tooltip("The prefeb to use for representing the player")]
  public GameObject playerPrefeb;
  [SerializeField]
  [Tooltip("The prefeb used to show time ,owned by maste client")]
  public GameObject informationPrefeb;
  public Joystick joystick;
  public GameObject weaponSystem;

  public Button AttackBtn;

  #endregion

  // Start is called before the first frame update
  void Start()
  {
    if (playerPrefeb == null)
    {
      Debug.LogError("Missing playerPrefeb reference");
    }
    else
    {
      if (movegetgromjoystick.LocalPlayerInstance == null)
      {
        // Instantiate the player
        GameObject thePlayer;
        if ((TeamController.Team)PhotonNetwork.LocalPlayer.CustomProperties["team"] == TeamController.Team.TeamA) {
          thePlayer = PhotonNetwork.Instantiate(this.playerPrefeb.name, new Vector3(-24, 5f, -46), Quaternion.identity, 0);
        }
        else
        {
          thePlayer = PhotonNetwork.Instantiate(this.playerPrefeb.name, new Vector3(24, 5f, 46), Quaternion.identity, 0);
        }

        // Assign components in the scene to the player we just instantiated, so that the player can
        // use those component

        // Assign joy stick
        thePlayer.GetComponent<movegetgromjoystick>().touch = joystick;

        // Set up player attack button
        AttackBtn.onClick.AddListener(thePlayer.GetComponent<PlayerCharacter>().Attack);

        // Assign the player to the weapon refresh places
        int nChild = weaponSystem.transform.childCount;
        Debug.Log("GameManager: there are " + nChild + " fresh places");
        //thePlayer.GetComponent<CharacterBehavior>().touch = joystick;
        GameObject[] children = new GameObject[nChild];
        WeaponRefresh wr;

        for (int i = 0; i < nChild; ++i)
        {
          children[i] = weaponSystem.transform.GetChild(i).gameObject;
          if (wr = children[i].GetComponent<WeaponRefresh>())
          {
            Debug.Log("GameManager: player assigned");
            wr.player = thePlayer.gameObject;
          }
        }

        // Assign the refresh places to the player
        thePlayer.GetComponent<PlayerCharacter>().refresh_places = children;

        // Assign the GameManager itself to the player so that the player can call the game manager
        thePlayer.GetComponent<PlayerCharacter>().GM = this;
      }
      else
      {
        // ignore
      }
    }

  }

  // Update is called once per frame
  void Update()
  {

  }
}
