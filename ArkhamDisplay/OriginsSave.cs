﻿using System;

namespace ArkhamDisplay{
	public class OriginsSave : SaveParser{
		public OriginsSave(string filePath, int id) : base(filePath, id){}

		public override bool HasKey(Entry entry, int requiredMatches){
			if("Data Handler".Equals(entry.type)){
				return HasKey("SS_Enigma_EnigmaHQ_Visited", requiredMatches) && !HasKey(entry.id, requiredMatches) && !HasKey(entry.alternateID, requiredMatches);
			}
			else if(!string.IsNullOrWhiteSpace(entry.metadata) && entry.metadata.Contains("DarkKnightFinish")){
				string id = entry.metadata.Contains("ALTERNATE") ? entry.alternateID : entry.id;
				return HasKeyCustomRegex(@"\b" + id + @"............" + Convert.ToChar(Convert.ToByte("0x1", 16)), requiredMatches);
			}

			return base.HasKey(entry, requiredMatches);
		}

		protected override string GetFile()
		{
			return System.IO.Path.Combine(m_filePath, "SpSave_v2_" + m_id.ToString() + ".sgd");
		}
	}
}