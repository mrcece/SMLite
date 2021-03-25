﻿using Fawdlstty.SMLite.ItemStruct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Fawdlstty.SMLite {
	public class SMLiteAsync<TState, TTrigger> where TState : Enum where TTrigger : Enum {
		internal SMLiteAsync (TState init_state, Dictionary<TState, _SMLite_ConfigStateAsync<TState, TTrigger>> _states) {
			State = init_state;
			m_states = _states;
		}

		public bool AllowTriggering (TTrigger trigger) {
			if (m_states.ContainsKey (State))
				return m_states[State]._allow_trigger (trigger);
			return false;
		}

		public async Task TriggeringAsync (TTrigger trigger, params object[] args) {
			await TriggeringAsync (trigger, CancellationToken.None, args);
		}

		public async Task TriggeringAsync (TTrigger trigger, CancellationToken token, params object[] args) {
			if (m_states.ContainsKey (State)) {
				if (m_states[State]._allow_trigger (trigger)) {
					var _old_state = State;
					try {
						var _new_state = await m_states[State]._triggerAsync (trigger, token, args);
						if (_new_state.CompareTo (State) != 0) {
							if (m_states[State].m_on_leave_async != null)
								await m_states[State].m_on_leave_async ();
							State = _new_state;
							if (m_states[State].m_on_entry_async != null)
								await m_states[State].m_on_entry_async ();
						}
						return;
					} catch (Exception) {
						State = _old_state;
						throw;
					}
				}
			}
			throw new Exception ("not match function found.");
		}

		public TState State { get; set; }
		Dictionary<TState, _SMLite_ConfigStateAsync<TState, TTrigger>> m_states = null;
	}
}
