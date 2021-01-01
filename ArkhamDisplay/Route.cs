﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ArkhamDisplay{
	public class Entry{
		public string name;
		public string type;
		public string id;
		public string alternateID;
		public string metadata;

		public Entry(string name, string type, string id, string alternateID = null, string metadata = null){
			this.name = name;
			this.type = type;
			this.id = id;
			this.alternateID = alternateID;
			this.metadata = metadata;
		}

		public bool IsType(string type_){
			if(string.IsNullOrWhiteSpace(type_)){
				return false;
			}

			return type_.Equals(type);
		}

		public static bool IsPlaceholder(Entry e){
			return e.IsType("[PLACEHOLDER]");
		}
	}

	public class Route{
		private string fileName;
		public List<Entry> entries;

		public Route(string file){
			fileName = file;
			entries = new List<Entry>();

			if(string.IsNullOrWhiteSpace(fileName)){
				throw new NullReferenceException();
			}

			if(!System.IO.File.Exists(fileName)){
				throw new System.IO.FileNotFoundException();
			}

			var allLines = System.IO.File.ReadAllLines(fileName).Skip(1);
			entries.Capacity = allLines.Count();
			foreach(string line in allLines){
				string[] lineComponents = line.Split('\t');
				if(lineComponents.Length < 3){
					throw new Exception("Could not load route! All entries must have at least 3 columns!");
				}

				string optionalAltID = null;
				string optionalMetaData = null;
				if(lineComponents.Length >= 4){
					optionalAltID = lineComponents[3].Trim();
				}
				if(lineComponents.Length >= 5){
					optionalMetaData = lineComponents[4].Trim();
				}

				entries.Add(new Entry(
					lineComponents[0].Trim(),
					lineComponents[1].Trim(),
					lineComponents[2].Trim(),
					optionalAltID,
					optionalMetaData
				));
			}
		}

		public List<Entry> GetEntriesWithPlaceholdersMoved(){
			List<Entry> newEntries = new List<Entry>(entries);

			List<Entry> placeHolders = newEntries.FindAll(Entry.IsPlaceholder);
			foreach(Entry p in placeHolders){
				List<Entry> onesToMove = newEntries.FindAll(x => x.IsType(p.name));
				newEntries.RemoveAll(x => x.IsType(p.name));
				newEntries.InsertRange(newEntries.FindIndex(x => x == p), onesToMove);
			}

			newEntries.RemoveAll(Entry.IsPlaceholder);
			return newEntries;
		}

		public List<Entry> GetEntriesWithoutPlaceholders(){
			List<Entry> newEntries = new List<Entry>(entries);
			newEntries.RemoveAll(Entry.IsPlaceholder);
			return newEntries;
		}
	}
}