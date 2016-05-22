using UnityEngine;
using System.Collections;

public class MauBinhInput {
    private ArrayCard[] cardChi;
    private Card card;
    private MauBinh mbStage;
    private MauBinhPlayer player;
    private ArrayCard cardHand;
    public MauBinhInput (MauBinh mbStage, MauBinhPlayer player, ArrayCard cardHand, ArrayCard[] cardChi, Card card) {
        this.cardHand = cardHand;
        this.cardChi = cardChi;
        this.card = card;
        this.mbStage = mbStage;
        this.player = player;
    }

    public void click () {
        if(!mbStage.isTouchCard) return;
        if(card.cardMo.gameObject.activeInHierarchy) {
            card.cardMo.gameObject.SetActive (false);
            return;
        }

        card.cardMo.gameObject.SetActive (true);
        GameControl.instance.sound.startchiabaiAudio ();
        Card c1 = null, c2 = null;
        int dem = 0;

        for(int i = 0; i < cardChi[0].getSize (); i++) {
            if(cardChi[0].getCardbyPos (i).cardMo.gameObject.activeInHierarchy) {
                dem++;
                if(dem == 1) {
                    c1 = cardChi[0].getCardbyPos (i);
                } else if(dem == 2) {
                    c2 = cardChi[0].getCardbyPos (i);
                }
            }
        }
        for(int i = 0; i < cardChi[1].getSize (); i++) {
            if(cardChi[1].getCardbyPos (i).cardMo.gameObject.activeInHierarchy) {
                dem++;
                if(dem == 1) {
                    c1 = cardChi[1].getCardbyPos (i);
                } else if(dem == 2) {
                    c2 = cardChi[1].getCardbyPos (i);
                }
            }
        }
        for(int i = 0; i < cardChi[2].getSize (); i++) {
            if(cardChi[2].getCardbyPos (i).cardMo.gameObject.activeInHierarchy) {
                dem++;
                if(dem == 1) {
                    c1 = cardChi[2].getCardbyPos (i);
                } else if(dem == 2) {
                    c2 = cardChi[2].getCardbyPos (i);
                }
            }
        }

        if(dem == 2) {
            int temp = c1.getId ();
            if(c1.getId () != c2.getId ()) {
                c1.setId (c2.getId ());
                c2.setId (temp);
            }
            changeMain ();
            change (0);
            change (1);
            change (2);

            int typecard1 = PokerCard.getTypeOfCardsPoker (cardChi[2].getArrayCardAllTrue ());
            int typecard2 = PokerCard.getTypeOfCardsPoker (cardChi[1].getArrayCardAllTrue ());
            int typecard3 = PokerCard.getTypeOfCardsPoker (cardChi[0].getArrayCardAllTrue ());

            mbStage.setLoaiBai (1, typecard1);
            mbStage.setLoaiBai (2, typecard2);
            mbStage.setLoaiBai (3, typecard3);

            mbStage.checkLung (cardChi[2].getArrayCardAllTrue (), cardChi[1].getArrayCardAllTrue (), cardChi[0].getArrayCardAllTrue ());
        }
    }

    void changeMain () {
        for(int i = 0; i < cardHand.getSize (); i++) {
            if(i < 5) {
                cardHand.getCardbyPos (i).setId (cardChi[2].getCardbyPos (i).getId ());
            } else if(i < 10) {
                cardHand.getCardbyPos (i).setId (cardChi[1].getCardbyPos (i - 5).getId ());
            } else {
                cardHand.getCardbyPos (i).setId (cardChi[0].getCardbyPos (i - 10).getId ());
            }
        }
    }

    void change (int index) {
        for(int i = 0; i < cardChi[index].getSize (); i++) {
            if(cardChi[index].getCardbyPos (i).cardMo.gameObject.activeInHierarchy) {
                cardChi[index].getCardbyPos (i).cardMo.gameObject.SetActive (false);
            }
        }
    }
}
