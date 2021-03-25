﻿using Fawdlstty.SMLite.ItemStruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fawdlstty.SMLite {
	public class SMLiteBuilderAsync<TState, TTrigger> where TState : Enum where TTrigger : Enum {
		public _SMLite_ConfigStateAsync<TState, TTrigger> Configure (TState state) {
			if (m_builded)
				throw new Exception ("shouldn't configure builder after builded.");
			if (m_states.ContainsKey (state))
				throw new Exception ("state is already exists.");
			var _state = new _SMLite_ConfigStateAsync<TState, TTrigger> { State = state };
			m_states.Add (state, _state);
			return _state;
		}

		public SMLiteAsync<TState, TTrigger> Build (TState init_state) {
			m_builded = true;
			return new SMLiteAsync<TState, TTrigger> (init_state, m_states);
		}

		Dictionary<TState, _SMLite_ConfigStateAsync<TState, TTrigger>> m_states = new Dictionary<TState, _SMLite_ConfigStateAsync<TState, TTrigger>> ();
		bool m_builded = false;
	}
}
