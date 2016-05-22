using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class BetMoney {
	public int typeMoney;
	public List<long> listBet = new List<long>();
	public long maxMoney;
	
	public BetMoney() {
		
	}
	
	public void setListBet(string str) {
		string[] result = str.Split(',');
		for (int i = 0; i < result.Length; i++) {
			listBet.Add(long.Parse(result[i]));
		}
	}
}
