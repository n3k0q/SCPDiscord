using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommandSystem;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Pickups;
using MapGeneration;
using MapGeneration.Distributors;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp939;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using RemoteAdmin;

namespace SCPDiscord.EventListeners
{
	internal class PlayerEventListener
	{
		private readonly SCPDiscord plugin;

		// First dimension is target player second dimension is attacking player
		private static readonly Dictionary<Team, Team> teamKillingMatrix = new Dictionary<Team, Team>
		{
			{ Team.FoundationForces, Team.Scientists },
			{ Team.ChaosInsurgency, Team.ClassD },
			{ Team.Scientists, Team.FoundationForces },
			{ Team.ClassD, Team.ChaosInsurgency }
		};

		public PlayerEventListener(SCPDiscord pl)
		{
			plugin = pl;
		}

		private bool IsTeamDamage(Team attackerTeam, Team targetTeam)
		{
			if (!plugin.roundStarted)
			{
				return false;
			}
			if (attackerTeam == targetTeam)
			{
				return true;
			}
			foreach (KeyValuePair<Team, Team> team in teamKillingMatrix)
			{
				if (attackerTeam == team.Value && targetTeam == team.Key)
				{
					return true;
				}
			}
			return false;
		}

		public enum DamageType
		{
			NONE = -1, // 0xFFFFFFFF
			RECONTAINED = 0,
			WARHEAD = 1,
			SCP_049 = 2,
			UNKNOWN = 3,
			ASPHYXIATED = 4,
			BLEEDING = 5,
			FALLING = 6,
			POCKET_DECAY = 7,
			DECONTAMINATION = 8,
			POISON = 9,
			SCP_207 = 10, // 0x0000000A
			SEVERED_HANDS = 11, // 0x0000000B
			MICRO_HID = 12, // 0x0000000C
			TESLA = 13, // 0x0000000D
			EXPLOSION = 14, // 0x0000000E
			SCP_096 = 15, // 0x0000000F
			SCP_173 = 16, // 0x00000010
			SCP_939 = 17, // 0x00000011
			SCP_049_2 = 18, // 0x00000012
			UNKNOWN_FIREARM = 19, // 0x00000013
			CRUSHED = 20, // 0x00000014
			FEMUR_BREAKER = 21, // 0x00000015
			FRIENDLY_FIRE_PUNISHMENT = 22, // 0x00000016
			HYPOTHERMIA = 23, // 0x00000017
			SCP_106 = 24, // 0x00000018
			SCP_018 = 25, // 0x00000019
			COM15 = 26, // 0x0000001A
			E11_SR = 27, // 0x0000001B
			CROSSVEC = 28, // 0x0000001C
			FSP9 = 29, // 0x0000001D
			LOGICER = 30, // 0x0000001E
			COM18 = 31, // 0x0000001F
			REVOLVER = 32, // 0x00000020
			AK = 33, // 0x00000021
			SHOTGUN = 34, // 0x00000022
			DISRUPTOR = 35, // 0x00000023
		}

		// Convert damage handler to smod style damage type
		private string GetDamageType(DamageHandlerBase handler)
		{
			switch (handler)
			{
				case DisruptorDamageHandler _:
					return "DISRUPTOR";

				case ExplosionDamageHandler explosionDamageHandler:
					return "EXPLOSION";

				case FirearmDamageHandler firearmDamageHandler:
					return firearmDamageHandler.WeaponType.ToString();

				case MicroHidDamageHandler microHidDamageHandler:
					return "MICRO_HID";

				case RecontainmentDamageHandler recontainmentDamageHandler:
					return "RECONTAINED";

				case Scp018DamageHandler scp018DamageHandler:
					return "SCP_018";

				case Scp049DamageHandler scp049DamageHandler:
					return "SCP_049";

				case Scp096DamageHandler scp096DamageHandler:
					return "SCP_096";

				case ScpDamageHandler scpDamageHandler:
					return "SCP_ATTACK";

				case Scp939DamageHandler scp939DamageHandler:
					return "SCP_939";

				//case AttackerDamageHandler attackerDamageHandler:
				//	break;

				case CustomReasonDamageHandler customReasonDamageHandler:
					return "UNKNOWN";

				case UniversalDamageHandler universalDamageHandler:
					return "UNKNOWN";

				case WarheadDamageHandler warheadDamageHandler:
					return "WARHEAD";

				//case StandardDamageHandler standardDamageHandler:
				//	break;

				default:
					return "UNKNOWN";
			}
		}

		[PluginEvent(ServerEventType.PlayerDamage)]
		public void OnPlayerHurt(Player attacker, Player target, DamageHandlerBase damageHandler)
		{
			if (target == null || target.Role == RoleTypeId.None || !(damageHandler is StandardDamageHandler stdHandler))
			{
				return;
			}

			if (attacker == null || target.PlayerId == attacker.PlayerId)
			{
				Dictionary<string, string> noAttackerVar = new Dictionary<string, string>
				{
					{ "damage",             stdHandler.Damage.ToString()             },
					{ "damagetype",         GetDamageType(damageHandler)             },
					{ "playeripaddress",    target.IpAddress                         },
					{ "playername",         target.Nickname                          },
					{ "playerplayerid",     target.PlayerId.ToString()               },
					{ "playersteamid",      target.GetParsedUserID()                 },
					{ "playerclass",        target.Role.ToString()                   },
					{ "playerteam",         target.ReferenceHub.GetTeam().ToString() }
				};
				plugin.SendMessage("messages.onplayerhurt.noattacker", noAttackerVar);
				return;
			}

			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "damage",             stdHandler.Damage.ToString()               },
				{ "damagetype",         GetDamageType(damageHandler)               },
				{ "attackeripaddress",  attacker.IpAddress                         },
				{ "attackername",       attacker.Nickname                          },
				{ "attackerplayerid",   attacker.PlayerId.ToString()               },
				{ "attackersteamid",    attacker.GetParsedUserID()                 },
				{ "attackerclass",      attacker.Role.ToString()                   },
				{ "attackerteam",       attacker.ReferenceHub.GetTeam().ToString() },
				{ "playeripaddress",    target.IpAddress                           },
				{ "playername",         target.Nickname                            },
				{ "playerplayerid",     target.PlayerId.ToString()                 },
				{ "playersteamid",      target.GetParsedUserID()                   },
				{ "playerclass",        target.Role.ToString()                     },
				{ "playerteam",         target.ReferenceHub.GetTeam().ToString()   }
			};

			if (IsTeamDamage(attacker.ReferenceHub.GetTeam(), target.ReferenceHub.GetTeam()))
			{
				plugin.SendMessage("messages.onplayerhurt.friendlyfire", variables);
				return;
			}

			plugin.SendMessage("messages.onplayerhurt.default", variables);
		}

		[PluginEvent(ServerEventType.PlayerDeath)]
		public void OnPlayerDie(Player target, Player attacker, DamageHandlerBase damageHandler)
		{
			if (target == null || target.Role == RoleTypeId.None || !(damageHandler is StandardDamageHandler stdHandler))
			{
				return;
			}

			if (attacker == null || target.PlayerId == attacker.PlayerId)
			{
				Dictionary<string, string> noKillerVar = new Dictionary<string, string>
				{
					{ "damagetype",         GetDamageType(damageHandler)         },
					{ "playeripaddress",    target.IpAddress                 },
					{ "playername",         target.Nickname                      },
					{ "playerplayerid",     target.PlayerId.ToString()       },
					{ "playersteamid",      target.GetParsedUserID()         },
					{ "playerclass",        target.Role.ToString()  },
					{ "playerteam",         target.ReferenceHub.GetTeam().ToString()  }
				};
				plugin.SendMessage("messages.onplayerdie.nokiller", noKillerVar);
				return;
			}

			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "damagetype",         GetDamageType(damageHandler)         },
				{ "attackeripaddress",  attacker.IpAddress                 },
				{ "attackername",       attacker.Nickname                      },
				{ "attackerplayerid",   attacker.PlayerId.ToString()       },
				{ "attackersteamid",    attacker.GetParsedUserID()         },
				{ "attackerclass",      attacker.Role.ToString()  },
				{ "attackerteam",       attacker.ReferenceHub.GetTeam().ToString()  },
				{ "playeripaddress",    target.IpAddress                 },
				{ "playername",         target.Nickname                      },
				{ "playerplayerid",     target.PlayerId.ToString()       },
				{ "playersteamid",      target.GetParsedUserID()         },
				{ "playerclass",        target.Role.ToString()  },
				{ "playerteam",         target.ReferenceHub.GetTeam().ToString()  }
			};

			if (IsTeamDamage(attacker.ReferenceHub.GetTeam(), target.ReferenceHub.GetTeam()))
			{
				plugin.SendMessage("messages.onplayerdie.friendlyfire", variables);
				return;
			}
			plugin.SendMessage("messages.onplayerdie.default", variables);
		}

		[PluginEvent(ServerEventType.PlayerPickupAmmo)]
		public void OnPlayerPickupAmmo(Player player, ItemPickupBase ammo)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ammo",         ammo.ToString()                    },
				{ "ipaddress",    player.IpAddress                   },
				{ "name",         player.Nickname                        },
				{ "playerid",     player.PlayerId.ToString()         },
				{ "steamid",      player.GetParsedUserID()                     },
				{ "class",        player.Role.ToString()    },
				{ "team",         player.ReferenceHub.GetTeam().ToString()    }
			};
			plugin.SendMessage("messages.onplayerpickupammo", variables);
		}

		[PluginEvent(ServerEventType.PlayerPickupArmor)]
		public void OnPlayerPickupArmor(Player player, ItemPickupBase armor)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "armor",         armor.ToString()                    },
				{ "ipaddress",    player.IpAddress                   },
				{ "name",         player.Nickname                        },
				{ "playerid",     player.PlayerId.ToString()         },
				{ "steamid",      player.GetParsedUserID()                     },
				{ "class",        player.Role.ToString()    },
				{ "team",         player.ReferenceHub.GetTeam().ToString()    }
			};
			plugin.SendMessage("messages.onplayerpickuparmor", variables);
		}

		[PluginEvent(ServerEventType.PlayerPickupScp330)]
		public void OnPlayerPickupSCP330(Player player, ItemPickupBase scp330)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ipaddress",    player.IpAddress                   },
				{ "name",         player.Nickname                        },
				{ "playerid",     player.PlayerId.ToString()         },
				{ "steamid",      player.GetParsedUserID()                     },
				{ "class",        player.Role.ToString()    },
				{ "team",         player.ReferenceHub.GetTeam().ToString()    }
			};
			plugin.SendMessage("messages.onplayerpickupscp330", variables);
		}

		[PluginEvent(ServerEventType.PlayerSearchedPickup)]
		public void OnPlayerPickupItem(Player player, ItemPickupBase item)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "item",         item.ToString()                    },
				{ "ipaddress",    player.IpAddress                   },
				{ "name",         player.Nickname                        },
				{ "playerid",     player.PlayerId.ToString()         },
				{ "steamid",      player.GetParsedUserID()                     },
				{ "class",        player.Role.ToString()    },
				{ "team",         player.ReferenceHub.GetTeam().ToString()    }
			};
			plugin.SendMessage("messages.onplayerpickupitem", variables);
		}

		[PluginEvent(ServerEventType.PlayerDropAmmo)]
		public void OnPlayerDropAmmo(Player player, ItemType ammo, int amount)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ammo",         ammo.ToString()                    },
				{ "amount",         amount.ToString()                    },
				{ "ipaddress",    player.IpAddress                   },
				{ "name",         player.Nickname                        },
				{ "playerid",     player.PlayerId.ToString()         },
				{ "steamid",      player.GetParsedUserID() },
				{ "class",        player.Role.ToString()    },
				{ "team",         player.ReferenceHub.GetTeam().ToString()    }
			};
			plugin.SendMessage("messages.onplayerdropammo", variables);
		}

		[PluginEvent(ServerEventType.PlayerDropItem)]
		public void OnPlayerDropItem(Player player, ItemBase item)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "item",         item.name                    },
				{ "ipaddress",    player.IpAddress                   },
				{ "name",         player.Nickname                        },
				{ "playerid",     player.PlayerId.ToString()         },
				{ "steamid",      player.GetParsedUserID() },
				{ "class",        player.Role.ToString()    },
				{ "team",         player.ReferenceHub.GetTeam().ToString()    }
			};
			plugin.SendMessage("messages.onplayerdropitem", variables);
		}

		[PluginEvent(ServerEventType.PlayerJoined)]
		public void OnPlayerJoin(Player player)
		{
			if (player.PlayerId == Server.Instance.PlayerId) return;

			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ipaddress",    player.IpAddress                          },
				{ "name",         player.Nickname                           },
				{ "playerid",     player.PlayerId.ToString()                },
				{ "steamid",      player.GetParsedUserID()                  },
				{ "class",        player.Role.ToString()                    },
				{ "team",         player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage("messages.onplayerjoin", variables);
		}

		[PluginEvent(ServerEventType.PlayerLeft)]
		public void OnPlayerLeave(Player player)
		{
			if (player.PlayerId == Server.Instance.PlayerId) return;

			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ipaddress", player.IpAddress           },
				{ "name", player.Nickname                 },
				{ "steamid", player.GetParsedUserID()     },
				{ "playerid", player.PlayerId.ToString()  }
			};
			this.plugin.SendMessage("messages.onplayerleave", variables);
		}

		/*
		[PluginEvent(ServerEventType.)]
		public void OnNicknameSet(PlayerNicknameSetEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "nickname",       ev.Nickname                         },
				{ "ipaddress",      ev.Player.IPAddress                 },
				{ "name",           ev.Player.Name                      },
				{ "playerid",       ev.Player.PlayerID.ToString()       },
				{ "steamid",        ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",          ev.Player.Role.ToString()  },
				{ "team",           ev.Player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage(Config.GetArray("messages.onnicknameset"), "messages.onnicknameset", variables);
		}
		*/

		[PluginEvent(ServerEventType.PlayerChangeRole)]
		public void OnSetRole(Player player, PlayerRoleBase oldRole, RoleTypeId newRole, RoleChangeReason changeReason)
		{
			if (newRole == RoleTypeId.None)
			{
				return;
			}

			// TODO: Split into different reasons
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ipaddress",      player.IpAddress                          },
				{ "name",           player.Nickname                           },
				{ "playerid",       player.PlayerId.ToString()                },
				{ "steamid",        player.GetParsedUserID()                  },
				{ "class",          player.Role.ToString()                    },
				{ "team",           player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage("messages.onsetrole", variables);
		}

		[PluginEvent(ServerEventType.PlayerSpawn)]
		public void OnSpawn(Player player, RoleTypeId role)
		{
			if (player == null || player.UserId == Server.Instance.UserId) return;

			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ipaddress",      player.IpAddress                         },
				{ "name",           player.Nickname                          },
				{ "playerid",       player.PlayerId.ToString()               },
				{ "steamid",        player.GetParsedUserID()                 },
				{ "class",          player.Role.ToString()                   },
				{ "team",           player.ReferenceHub.GetTeam().ToString() }
			};

			plugin.SendMessage("messages.onspawn", variables);
		}

		/*
		public void OnDoorAccess(PlayerDoorAccessEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "doorname",       ev.Door.Name                          },
				{ "permission",     ev.Door.RequiredPermission.ToString() },
				{ "locked",         ev.Door.IsLocked.ToString()           },
				{ "open",           ev.Door.IsOpen.ToString()             },
				{ "ipaddress",      ev.Player.IPAddress                   },
				{ "name",           ev.Player.Name                        },
				{ "playerid",       ev.Player.PlayerID.ToString()         },
				{ "steamid",        ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",          ev.Player.Role.ToString()      },
				{ "team",           ev.Player.ReferenceHub.GetTeam().ToString()    }
			};
			if (ev.Allow)
			{
				plugin.SendMessage(Config.GetArray("messages.ondooraccess.allowed"), "messages.ondooraccess.allowed", variables);
			}
			else
			{
				plugin.SendMessage(Config.GetArray("messages.ondooraccess.denied"), "messages.ondooraccess.denied", variables);
			}
		}
		*/

		/*
		public void OnIntercom(PlayerIntercomEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "cooldowntime",   ev.CooldownTime.ToString()          },
				{ "speechtime",     ev.SpeechTime.ToString()            },
				{ "ipaddress",      ev.Player.IPAddress                 },
				{ "name",           ev.Player.Name                      },
				{ "playerid",       ev.Player.PlayerID.ToString()       },
				{ "steamid",        ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",          ev.Player.Role.ToString()    },
				{ "team",           ev.Player.ReferenceHub.GetTeam().ToString()  }
			};

			plugin.SendMessage(Config.GetArray("messages.onintercom"), "messages.onintercom", variables);
		}
		*/

		/*
		public void OnPocketDimensionExit(PlayerPocketDimensionExitEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ipaddress",          ev.Player.IPAddress                 },
				{ "name",               ev.Player.Name                      },
				{ "playerid",           ev.Player.PlayerID.ToString()       },
				{ "steamid",            ev.Player.GetParsedUserID()  ?? ev.Player.UserID },
				{ "class",              ev.Player.Role.ToString()    },
				{ "team",               ev.Player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage(Config.GetArray("messages.onpocketdimensionexit"), "messages.onpocketdimensionexit", variables);
		}
		*/

		/*
		public void OnPocketDimensionEnter(PlayerPocketDimensionEnterEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "damage",             ev.Damage.ToString()                },
				{ "attackeripaddress",  ev.Attacker.IPAddress               },
				{ "attackername",       ev.Attacker.Name                    },
				{ "attackerplayerid",   ev.Attacker.PlayerID.ToString()     },
				{ "attackersteamid",    ev.Attacker.GetParsedUserID() ?? ev.Player.UserID },
				{ "attackerclass",      ev.Attacker.Role.ToString()},
				{ "attackerteam",       ev.Attacker.ReferenceHub.GetTeam().ToString()},
				{ "playeripaddress",    ev.Player.IPAddress                 },
				{ "playername",         ev.Player.Name                      },
				{ "playerplayerid",     ev.Player.PlayerID.ToString()       },
				{ "playersteamid",      ev.Player.GetParsedUserID()  ?? ev.Player.UserID },
				{ "playerclass",        ev.Player.Role.ToString()    },
				{ "playerteam",         ev.Player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage(Config.GetArray("messages.onpocketdimensionenter"), "messages.onpocketdimensionenter", variables);
		}
		*/

		/*
		public void OnPocketDimensionDie(PlayerPocketDimensionDieEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ipaddress",          ev.Player.IPAddress                 },
				{ "name",               ev.Player.Name                      },
				{ "playerid",           ev.Player.PlayerID.ToString()       },
				{ "steamid",            ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",              ev.Player.Role.ToString()    },
				{ "team",               ev.Player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage(Config.GetArray("messages.onpocketdimensiondie"), "messages.onpocketdimensiondie", variables);
		}
		*/

		/*
		public void OnThrowGrenade(PlayerThrowGrenadeEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "type",               ev.GrenadeType.ToString()           },
				{ "ipaddress",          ev.Player.IPAddress                 },
				{ "name",               ev.Player.Name                      },
				{ "playerid",           ev.Player.PlayerID.ToString()       },
				{ "steamid",            ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",              ev.Player.Role.ToString()  },
				{ "team",               ev.Player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage(Config.GetArray("messages.onthrowgrenade"), "messages.onthrowgrenade", variables);
		}
		*/

		/*
		public void OnPlayerInfected(PlayerInfectedEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "damage",                 ev.Damage.ToString()                    },
				{ "infecttime",             ev.InfectTime.ToString()                },
				{ "attackeripaddress",      ev.Attacker.IPAddress                   },
				{ "attackername",           ev.Attacker.Name                        },
				{ "attackerplayerid",       ev.Attacker.PlayerID.ToString()         },
				{ "attackersteamid",        ev.Attacker.GetParsedUserID() ?? ev.Player.UserID },
				{ "attackerclass",          ev.Attacker.Role.ToString()      },
				{ "attackerteam",           ev.Attacker.ReferenceHub.GetTeam().ToString()    },
				{ "playeripaddress",        ev.Attacker.IPAddress                   },
				{ "playername",             ev.Player.Name                          },
				{ "playerplayerid",         ev.Player.PlayerID.ToString()           },
				{ "playersteamid",          ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "playerclass",            ev.Player.Role.ToString()        },
				{ "playerteam",             ev.Player.ReferenceHub.GetTeam().ToString()      }
			};
			plugin.SendMessage(Config.GetArray("messages.onplayerinfected"), "messages.onplayerinfected", variables);
		}
		*/

		[PluginEvent(ServerEventType.RagdollSpawn)]
		public void OnSpawnRagdoll(Player player, IRagdollRole ragdollRole, DamageHandlerBase damageHandler)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "damagetype",         GetDamageType(damageHandler)              },
				{ "ipaddress",          player.IpAddress                          },
				{ "name",               player.Nickname                           },
				{ "playerid",           player.PlayerId.ToString()                },
				{ "steamid",            player.GetParsedUserID()                  },
				{ "class",              player.ToString()                         },
				{ "team",               player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage("messages.onspawnragdoll", variables);
		}

		/*
		public void OnLure(PlayerLureEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "allowcontain",       ev.AllowContain.ToString()          },
				{ "ipaddress",          ev.Player.IPAddress                 },
				{ "name",               ev.Player.Name                      },
				{ "playerid",           ev.Player.PlayerID.ToString()       },
				{ "steamid",            ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",              ev.Player.Role.ToString()  },
				{ "team",               ev.Player.ReferenceHub.GetTeam().ToString()  }
			};

			plugin.SendMessage(Config.GetArray("messages.onlure"), "messages.onlure", variables);
		}
		*/

		/*
		public void OnContain106(PlayerContain106Event ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "activatecontainment",    ev.ActivateContainment.ToString()   },
				{ "ipaddress",              ev.Player.IPAddress                 },
				{ "name",                   ev.Player.Name                      },
				{ "playerid",               ev.Player.PlayerID.ToString()       },
				{ "steamid",                ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",                  ev.Player.Role.ToString()  },
				{ "team",                   ev.Player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage(Config.GetArray("messages.oncontain106"), "messages.oncontain106", variables);
		}
		*/

		/*
		public void OnConsumableUse(PlayerConsumableUseEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "health",                 ev.Health.ToString()               },
				{ "artificialhealth",       ev.ArtificialHealth.ToString()     },
				{ "healthregenamount",            ev.HealthRegenAmount.ToString()    },
				{ "healthregenspeedmultiplier",   ev.HealthRegenSpeedMultiplier.ToString() },
				{ "stamina",                ev.Stamina.ToString()              },
				{ "medicalitem",            ev.ConsumableItem.ToString()       },
				{ "ipaddress",              ev.Player.IPAddress                },
				{ "name",                   ev.Player.Name                     },
				{ "playerid",               ev.Player.PlayerID.ToString()      },
				{ "steamid",                ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",                  ev.Player.Role.ToString() },
				{ "team",                   ev.Player.ReferenceHub.GetTeam().ToString() }
			};
			plugin.SendMessage(Config.GetArray("messages.onmedicaluse"), "messages.onmedicaluse", variables);
		}
		*/

		/*
		public void On106CreatePortal(Player106CreatePortalEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ipaddress",              ev.Player.IPAddress                 },
				{ "name",                   ev.Player.Name                      },
				{ "playerid",               ev.Player.PlayerID.ToString()       },
				{ "steamid",                ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",                  ev.Player.Role.ToString()  },
				{ "team",                   ev.Player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage(Config.GetArray("messages.on106createportal"), "messages.on106createportal", variables);
		}
		*/

		/*
		public void On106Teleport(Player106TeleportEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ipaddress",              ev.Player.IPAddress                 },
				{ "name",                   ev.Player.Name                      },
				{ "playerid",               ev.Player.PlayerID.ToString()       },
				{ "steamid",                ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",                  ev.Player.Role.ToString()  },
				{ "team",                   ev.Player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage(Config.GetArray("messages.on106teleport"), "messages.on106teleport", variables);
		}
		*/

		/*
		public void OnElevatorUse(PlayerElevatorUseEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "elevatorname",           ev.Elevator.ElevatorType.ToString() },
				{ "ipaddress",              ev.Player.IPAddress                 },
				{ "name",                   ev.Player.Name                      },
				{ "playerid",               ev.Player.PlayerID.ToString()       },
				{ "steamid",                ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",                  ev.Player.Role.ToString()  },
				{ "team",                   ev.Player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage(Config.GetArray("messages.onelevatoruse"), "messages.onelevatoruse", variables);
		}
		*/

		/*
		public void OnHandcuffed(PlayerHandcuffedEvent ev)
		{
			if (ev.Disarmer != null)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "cuffed",             ev.Allow.ToString()                     },
					{ "targetipaddress",    ev.Player.IPAddress                     },
					{ "targetname",         ev.Player.Name                          },
					{ "targetplayerid",     ev.Player.PlayerID.ToString()           },
					{ "targetsteamid",      ev.Player.GetParsedUserID() ?? ev.Player.UserID },
					{ "targetclass",        ev.Player.Role.ToString()   },
					{ "targetteam",         ev.Player.ReferenceHub.GetTeam().ToString()     },
					{ "playeripaddress",    ev.Disarmer.IPAddress                    },
					{ "playername",         ev.Disarmer.Name                         },
					{ "playerplayerid",     ev.Disarmer.PlayerID.ToString()          },
					{ "playersteamid",      ev.Disarmer.GetParsedUserID() ?? ev.Player.UserID },
					{ "playerclass",        ev.Disarmer.Role.ToString() },
					{ "playerteam",         ev.Disarmer.ReferenceHub.GetTeam().ToString()   }
				};
				plugin.SendMessage(Config.GetArray("messages.onhandcuff.default"), "messages.onhandcuff.default", variables);
			}
			else
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "cuffed",             ev.Allow.ToString()                     },
					{ "targetipaddress",    ev.Player.IPAddress                     },
					{ "targetname",         ev.Player.Name                          },
					{ "targetplayerid",     ev.Player.PlayerID.ToString()           },
					{ "targetsteamid",      ev.Player.GetParsedUserID() ?? ev.Player.UserID },
					{ "targetclass",        ev.Player.Role.ToString()        },
					{ "targetteam",         ev.Player.ReferenceHub.GetTeam().ToString()      }
				};
				plugin.SendMessage(Config.GetArray("messages.onhandcuff.nootherplayer"), "messages.onhandcuff.nootherplayer", variables);
			}
		}
		*/

		/*
		public void OnPlayerTriggerTesla(PlayerTriggerTeslaEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ipaddress",              ev.Player.IPAddress                 },
				{ "name",                   ev.Player.Name                      },
				{ "playerid",               ev.Player.PlayerID.ToString()       },
				{ "steamid",                ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",                  ev.Player.Role.ToString()    },
				{ "team",                   ev.Player.ReferenceHub.GetTeam().ToString()  }
			};

			if (ev.Triggerable)
			{
				plugin.SendMessage(Config.GetArray("messages.onplayertriggertesla.default"), "messages.onplayertriggertesla.default", variables);
			}
			else
			{
				plugin.SendMessage(Config.GetArray("messages.onplayertriggertesla.ignored"), "messages.onplayertriggertesla.ignored", variables);
			}
		}
		*/

		/*
		public void OnSCP914ChangeKnob(PlayerSCP914ChangeKnobEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "setting",                ev.KnobSetting.ToString()           },
				{ "ipaddress",              ev.Player.IPAddress                 },
				{ "name",                   ev.Player.Name                      },
				{ "playerid",               ev.Player.PlayerID.ToString()       },
				{ "steamid",                ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",                  ev.Player.Role.ToString()    },
				{ "team",                   ev.Player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage(Config.GetArray("messages.onscp914changeknob"), "messages.onscp914changeknob", variables);
		}
		*/

		/*
		public void OnPlayerRadioSwitch(PlayerRadioSwitchEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "setting",                ev.ChangeTo.ToString()              },
				{ "ipaddress",              ev.Player.IPAddress                 },
				{ "name",                   ev.Player.Name                      },
				{ "playerid",               ev.Player.PlayerID.ToString()       },
				{ "steamid",                ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",                  ev.Player.Role.ToString()    },
				{ "team",                   ev.Player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage(Config.GetArray("messages.onplayerradioswitch"), "messages.onplayerradioswitch", variables);
		}
		*/

		/*
		public void OnRecallZombie(PlayerRecallZombieEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "allowrecall",        ev.AllowRecall.ToString()          },
				{ "playeripaddress",    ev.Player.IPAddress                },
				{ "playername",         ev.Player.Name                     },
				{ "playerplayerid",     ev.Player.PlayerID.ToString()      },
				{ "playersteamid",      ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "playerclass",        ev.Player.Role.ToString()   },
				{ "playerteam",         ev.Player.ReferenceHub.GetTeam().ToString() },
				{ "targetipaddress",    ev.Target.IPAddress                },
				{ "targetname",         ev.Target.Name                     },
				{ "targetplayerid",     ev.Target.PlayerID.ToString()      },
				{ "targetsteamid",      ev.Target.GetParsedUserID() ?? ev.Player.UserID },
				{ "targetclass",        ev.Target.Role.ToString()   },
				{ "targetteam",         ev.Target.ReferenceHub.GetTeam().ToString() },
			};
			plugin.SendMessage(Config.GetArray("messages.onrecallzombie"), "messages.onrecallzombie", variables);
		}
		*/

		[PluginEvent(ServerEventType.RemoteAdminCommandExecuted)]
		public void OnRemoteAdminCommand(ICommandSender commandSender, string command, string[] args, bool result, string response)
		{
			if (commandSender is PlayerCommandSender playerSender && Player.Get(playerSender.ReferenceHub) != null)
			{
				Player player = Player.Get(playerSender.ReferenceHub);
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "command",       command + " " + string.Join(" ", args)   },
					{ "result",        result.ToString()                        },
					{ "returnmessage", response                                 },
					{ "ipaddress",     player.IpAddress                         },
					{ "name",          player.Nickname                          },
					{ "playerid",      player.PlayerId.ToString()               },
					{ "steamid",       player.GetParsedUserID()                 },
					{ "class",         player.Role.ToString()                   },
					{ "team",          player.ReferenceHub.GetTeam().ToString() }
				};
				plugin.SendMessage("messages.onexecutedcommand.remotemessages.player", variables);
			}
			else
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "command",       command + " " + string.Join(" ", args)   },
					{ "result",        result.ToString()                        },
					{ "returnmessage", response                                 }
				};
				plugin.SendMessage("messages.onexecutedcommand.remotemessages.server", variables);
			}
		}

		[PluginEvent(ServerEventType.PlayerGameConsoleCommandExecuted)]
		public void OnGameConsoleCommand(Player player, string command, string[] args, string response)
		{
			if (player != null && player.PlayerId != Server.Instance.PlayerId)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "command",       command + " " + string.Join(" ", args)   },
					//{ "result",        result.ToString()                        },
					{ "returnmessage", response                                 },
					{ "ipaddress",     player.IpAddress                         },
					{ "name",          player.Nickname                          },
					{ "playerid",      player.PlayerId.ToString()               },
					{ "steamid",       player.GetParsedUserID()                 },
					{ "class",         player.Role.ToString()                   },
					{ "team",          player.ReferenceHub.GetTeam().ToString() }
				};
				plugin.SendMessage("messages.onexecutedcommand.game.player", variables);
			}
			else
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "command",       command + " " + string.Join(" ", args) },
					//{ "result",        result.ToString()                      },
					{ "returnmessage", response                               }
				};
				plugin.SendMessage("messages.onexecutedcommand.game.server", variables);
			}
		}

		[PluginEvent(ServerEventType.ConsoleCommandExecuted)]
		public void OnConsoleCommand(ICommandSender commandSender, string command, string[] args, bool result, string response)
		{
			if (commandSender is PlayerCommandSender playerSender && Player.Get(playerSender.ReferenceHub) != null)
			{
				Player player = Player.Get(playerSender.ReferenceHub);
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "command",       command + " " + string.Join(" ", args)   },
					{ "result",        result.ToString()                        },
					{ "ipaddress",     player.IpAddress                         },
					{ "name",          player.Nickname                          },
					{ "playerid",      player.PlayerId.ToString()               },
					{ "steamid",       player.GetParsedUserID()                 },
					{ "class",         player.Role.ToString()                   },
					{ "team",          player.ReferenceHub.GetTeam().ToString() },
					{ "result",        result.ToString()                        },
					{ "returnmessage", response                                 }
				};
				plugin.SendMessage("messages.onexecutedcommand.console.player", variables);
			}
			else
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "command",       command + " " + string.Join(" ", args)   },
					{ "result",        result.ToString()                        },
					{ "returnmessage", response                                 }
				};
				plugin.SendMessage("messages.onexecutedcommand.console.server", variables);
			}
		}

		[PluginEvent(ServerEventType.RemoteAdminCommand)]
		public void OnRemoteAdminCommand(ICommandSender commandSender, string command, string[] args)
		{
			if (commandSender is PlayerCommandSender playerSender && Player.Get(playerSender.ReferenceHub) != null)
			{
				Player player = Player.Get(playerSender.ReferenceHub);
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "command",   command + " " + string.Join(" ", args)   },
					{ "ipaddress", player.IpAddress                         },
					{ "name",      player.Nickname                          },
					{ "playerid",  player.PlayerId.ToString()               },
					{ "steamid",   player.GetParsedUserID()                 },
					{ "class",     player.Role.ToString()                   },
					{ "team",      player.ReferenceHub.GetTeam().ToString() }
				};
				plugin.SendMessage("messages.oncallcommand.remotemessages.player", variables);
			}
			else
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "command",   command + " " + string.Join(" ", args)   }
				};
				plugin.SendMessage("messages.oncallcommand.remotemessages.server", variables);
			}
		}

		[PluginEvent(ServerEventType.PlayerGameConsoleCommand)]
		public void OnGameConsoleCommand(Player player, string command, string[] args)
		{
			if (player != null && player.PlayerId != Server.Instance.PlayerId)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "command",   command + " " + string.Join(" ", args)   },
					{ "ipaddress", player.IpAddress                         },
					{ "name",      player.Nickname                          },
					{ "playerid",  player.PlayerId.ToString()               },
					{ "steamid",   player.GetParsedUserID()                 },
					{ "class",     player.Role.ToString()                   },
					{ "team",      player.ReferenceHub.GetTeam().ToString() }
				};
				plugin.SendMessage("messages.oncallcommand.game.player", variables);
			}
			else
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "command",   command + " " + string.Join(" ", args)   }
				};
				plugin.SendMessage("messages.oncallcommand.game.server", variables);
			}
		}

		[PluginEvent(ServerEventType.ConsoleCommand)]
		public void OnConsoleCommand(ICommandSender commandSender, string command, string[] args)
		{
			if (commandSender is PlayerCommandSender playerSender && Player.Get(playerSender.ReferenceHub) != null)
			{
				Player player = Player.Get(playerSender.ReferenceHub);
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "command",   command + " " + string.Join(" ", args)   },
					{ "ipaddress", player.IpAddress                         },
					{ "name",      player.Nickname                          },
					{ "playerid",  player.PlayerId.ToString()               },
					{ "steamid",   player.GetParsedUserID()                 },
					{ "class",     player.Role.ToString()                   },
					{ "team",      player.ReferenceHub.GetTeam().ToString() }
				};
				plugin.SendMessage("messages.oncallcommand.console.player", variables);
			}
			else
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "command", command + " " + string.Join(" ", args)   }
				};
				plugin.SendMessage("messages.oncallcommand.console.server", variables);
			}
		}

		[PluginEvent(ServerEventType.PlayerReloadWeapon)]
		public void OnReload(Player player, Firearm weapon)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "weapon",      weapon.name                                 },
				{ "maxclipsize", weapon.AmmoManagerModule.MaxAmmo.ToString() },
				{ "ipaddress",   player?.IpAddress                           },
				{ "name",        player?.Nickname                            },
				{ "playerid",    player?.PlayerId.ToString()                 },
				{ "steamid",     player?.GetParsedUserID()                   },
				{ "class",       player?.Role.ToString()                     },
				{ "team",        player?.ReferenceHub.GetTeam().ToString()   }
			};
			plugin.SendMessage("messages.onreload", variables);
		}

		[PluginEvent(ServerEventType.GrenadeExploded)]
		public void OnGrenadeExplosion(ItemPickupBase grenade)
		{
			/*
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ipaddress",                  ev.Player?.IPAddress                },
				{ "name",                       ev.Player?.Name                     },
				{ "playerid",                   ev.Player?.PlayerID.ToString()      },
				{ "steamid",                    ev.Player?.GetParsedUserID()        },
				{ "class",                      ev.Player?.Role.ToString()   },
				{ "team",                       ev.Player?.ReferenceHub.GetTeam().ToString() }
			};
			plugin.SendMessage("messages.ongrenadeexplosion", variables);
			*/
		}

		/*
		public void OnGrenadeHitPlayer(PlayerGrenadeHitPlayer ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "playeripaddress",    ev.Player?.IPAddress                 },
				{ "playername",         ev.Player?.Name                      },
				{ "playerplayerid",     ev.Player?.PlayerID.ToString()       },
				{ "playersteamid",      ev.Player?.GetParsedUserID() ?? ev.Player?.UserID },
				{ "playerclass",        ev.Player?.PlayerRole?.RoleID.ToString()    },
				{ "playerteam",         ev.Player?.PlayerRole?.Team.ToString()  },
				{ "targetipaddress",    ev.Victim?.IPAddress                 },
				{ "targetname",         ev.Victim?.Name                      },
				{ "targetplayerid",     ev.Victim?.PlayerID.ToString()       },
				{ "targetsteamid",      ev.Victim?.GetParsedUserID() ?? ev.Victim?.UserID },
				{ "targetclass",        ev.Victim?.Role.ToString()    },
				{ "targetteam",         ev.Victim?.ReferenceHub.GetTeam().ToString()  },
			};
			plugin.SendMessage(Config.GetArray("messages.ongrenadehitplayer"), "messages.ongrenadehitplayer", variables);
		}
		*/

		[PluginEvent(ServerEventType.PlayerUnlockGenerator)]
		public void OnGeneratorUnlock(Player player, Scp079Generator generator)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "engaged",                    generator.Engaged.ToString()              },
				{ "activating",                 generator.Activating.ToString()           },
				//{ "locked",                     (!generator.).ToString()                  },
				//{ "open",                       generator.IsOpen.ToString()               },
				{ "room",                       generator.GetComponentInParent<RoomIdentifier>().Name.ToString() },
				//{ "starttime",                  generator.ActivationTime.ToString()       },
				//{ "timeleft",                   generator.ActivationTimeLeft.ToString()   },
				{ "ipaddress",                  player.IpAddress                          },
				{ "name",                       player.Nickname                           },
				{ "playerid",                   player.PlayerId.ToString()                },
				{ "steamid",                    player.GetParsedUserID()                  },
				{ "class",                      player.Role.ToString()                    },
				{ "team",                       player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage("messages.ongeneratorunlock", variables);
		}

		[PluginEvent(ServerEventType.PlayerOpenGenerator)]
		public void OnGeneratorOpen(Player player, Scp079Generator generator)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "engaged",          generator.Engaged.ToString()                 },
				{ "activating",       generator.Activating.ToString()              },
				//{ "locked",           (!ev.Generator.IsUnlocked).ToString()        },
				//{ "open",             generator.IsOpen.ToString()                  },
				{ "room",             generator.GetComponentInParent<RoomIdentifier>().Name.ToString() },
				//{ "starttime",        ev.Generator.ActivationTime.ToString()       },
				//{ "timeleft",         ev.Generator.ActivationTimeLeft.ToString()   },
				{ "ipaddress",        player.IpAddress                             },
				{ "name",             player.Nickname                              },
				{ "playerid",         player.PlayerId.ToString()                   },
				{ "steamid",          player.GetParsedUserID()                     },
				{ "class",            player.Role.ToString()                       },
				{ "team",             player.ReferenceHub.GetTeam().ToString()     }
			};

			plugin.SendMessage("messages.ongeneratoropen", variables);
		}

		[PluginEvent(ServerEventType.PlayerCloseGenerator)]
		public void OnGeneratorClose(Player player, Scp079Generator generator)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "engaged",        generator.Engaged.ToString()                  },
				{ "activating",     generator.Activating.ToString()               },
				//{ "locked",         (!ev.Generator.IsUnlocked).ToString()         },
				//{ "open",           generator.IsOpen.ToString()                   },
				{ "room",           generator.GetComponentInParent<RoomIdentifier>().Name.ToString() },
				//{ "starttime",      ev.Generator.ActivationTime.ToString()        },
				//{ "timeleft",       ev.Generator.ActivationTimeLeft.ToString()    },
				{ "ipaddress",      player.IpAddress                              },
				{ "name",           player.Nickname                               },
				{ "playerid",       player.PlayerId.ToString()                    },
				{ "steamid",        player.GetParsedUserID()                      },
				{ "class",          player.Role.ToString()                        },
				{ "team",           player.ReferenceHub.GetTeam().ToString()      }
			};
			plugin.SendMessage("messages.ongeneratorclose", variables);
		}

		[PluginEvent(ServerEventType.PlayerActivateGenerator)]
		public void OnGeneratorActivated(Player player, Scp079Generator generator)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "engaged",       generator.Engaged.ToString()                },
				{ "activating",    generator.Activating.ToString()             },
				//{ "locked",        (!ev.Generator.IsUnlocked).ToString()       },
				//{ "open",          ev.Generator.IsOpen.ToString()              },
				{ "room",          generator.GetComponentInParent<RoomIdentifier>().Name.ToString() },
				//{ "starttime",     ev.Generator.ActivationTime.ToString()      },
				//{ "timeleft",      ev.Generator.ActivationTimeLeft.ToString()  },
				{ "ipaddress",     player.IpAddress                            },
				{ "name",          player.Nickname                             },
				{ "playerid",      player.PlayerId.ToString()                  },
				{ "steamid",       player.GetParsedUserID()                    },
				{ "class",         player.Role.ToString()                      },
				{ "team",          player.ReferenceHub.GetTeam().ToString()    }
			};
			plugin.SendMessage("messages.ongeneratoractivated", variables);
		}

		[PluginEvent(ServerEventType.PlayerDeactivatedGenerator)]
		public void OnGeneratorDeactivated(Player player, Scp079Generator generator)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "engaged",       generator.Engaged.ToString()                },
				{ "activating",    generator.Activating.ToString()             },
				//{ "locked",        (!ev.Generator.IsUnlocked).ToString()       },
				//{ "open",          ev.Generator.IsOpen.ToString()              },
				{ "room",          generator.GetComponentInParent<RoomIdentifier>().Name.ToString() },
				//{ "starttime",     ev.Generator.ActivationTime.ToString()      },
				//{ "timeleft",      ev.Generator.ActivationTimeLeft.ToString()  },
				{ "ipaddress",     player.IpAddress                            },
				{ "name",          player.Nickname                             },
				{ "playerid",      player.PlayerId.ToString()                  },
				{ "steamid",       player.GetParsedUserID()                    },
				{ "class",         player.Role.ToString()                      },
				{ "team",          player.ReferenceHub.GetTeam().ToString()    }
			};
			plugin.SendMessage("messages.ongeneratordeactivated", variables);
		}

		/*
		public void On079Door(Player079DoorEvent ev)
		{
			if (ev.Allow)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "apdrain",                    ev.APDrain.ToString()              },
					{ "door",                       ev.Door.Name                       },
					{ "open",                       ev.Door.IsOpen.ToString()          },
					{ "ipaddress",                  ev.Player.IPAddress                },
					{ "name",                       ev.Player.Name                     },
					{ "playerid",                   ev.Player.PlayerID.ToString()      },
					{ "steamid",                    ev.Player.GetParsedUserID() ?? ev.Player.UserID },
					{ "class",                      ev.Player.Role.ToString()   },
					{ "team",                       ev.Player.ReferenceHub.GetTeam().ToString() }
				};
				if (ev.Door.IsOpen)
				{
					plugin.SendMessage(Config.GetArray("messages.on079door.closed"), "messages.on079door.closed", variables);
				}
				else
				{
					plugin.SendMessage(Config.GetArray("messages.on079door.opened"), "messages.on079door.opened", variables);
				}
			}
		}
		*/

		/*
		public void On079Lock(Player079LockEvent ev)
		{
			if (ev.Allow)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "apdrain",                    ev.APDrain.ToString()               },
					{ "door",                       ev.Door.Name                        },
					{ "open",                       ev.Door.IsOpen.ToString()           },
					{ "ipaddress",                  ev.Player.IPAddress                 },
					{ "name",                       ev.Player.Name                      },
					{ "playerid",                   ev.Player.PlayerID.ToString()       },
					{ "steamid",                    ev.Player.GetParsedUserID() ?? ev.Player.UserID },
					{ "class",                      ev.Player.Role.ToString()    },
					{ "team",                       ev.Player.ReferenceHub.GetTeam().ToString()  }
				};
				if (ev.Door.IsLocked)
				{
					plugin.SendMessage(Config.GetArray("messages.on079lock.unlocked"), "messages.on079lock.unlocked", variables);
				}
				else
				{
					plugin.SendMessage(Config.GetArray("messages.on079lock.locked"), "messages.on079lock.locked", variables);
				}
			}
		}
		*/

		/*
		public void On079Elevator(Player079ElevatorEvent ev)
		{
			if (ev.Allow)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "apdrain",                    ev.APDrain.ToString()                },
					{ "elevator",                   ev.Elevator.ElevatorType.ToString()  },
					{ "status",                     ev.Elevator.ElevatorStatus.ToString()},
					{ "ipaddress",                  ev.Player.IPAddress                  },
					{ "name",                       ev.Player.Name                       },
					{ "playerid",                   ev.Player.PlayerID.ToString()        },
					{ "steamid",                    ev.Player.GetParsedUserID() ?? ev.Player.UserID },
					{ "class",                      ev.Player.Role.ToString()     },
					{ "team",                       ev.Player.ReferenceHub.GetTeam().ToString()   }
				};
				if (ev.Elevator.ElevatorStatus == ElevatorStatus.DOWN)
				{
					plugin.SendMessage(Config.GetArray("messages.on079elevator.up"), "messages.on079elevator.up", variables);
				}
				else if (ev.Elevator.ElevatorStatus == ElevatorStatus.UP)
				{
					plugin.SendMessage(Config.GetArray("messages.on079elevator.down"), "messages.on079elevator.down", variables);
				}
			}
		}
		*/

		/*
		public void On079TeslaGate(Player079TeslaGateEvent ev)
		{
			if (ev.Allow)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "apdrain",                    ev.APDrain.ToString()               },
					{ "ipaddress",                  ev.Player.IPAddress                 },
					{ "name",                       ev.Player.Name                      },
					{ "playerid",                   ev.Player.PlayerID.ToString()       },
					{ "steamid",                    ev.Player.GetParsedUserID() ?? ev.Player.UserID },
					{ "class",                      ev.Player.Role.ToString()    },
					{ "team",                       ev.Player.ReferenceHub.GetTeam().ToString()  }
				};
				plugin.SendMessage(Config.GetArray("messages.on079teslagate"), "messages.on079teslagate", variables);
			}
		}
		*/

		/*
		public void On079AddExp(Player079AddExpEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "xptype",                     ev.ExperienceType.ToString()        },
				{ "amount",                     ev.ExpToAdd.ToString()              },
				{ "ipaddress",                  ev.Player.IPAddress                 },
				{ "name",                       ev.Player.Name                      },
				{ "playerid",                   ev.Player.PlayerID.ToString()       },
				{ "steamid",                    ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",                      ev.Player.Role.ToString()    },
				{ "team",                       ev.Player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage(Config.GetArray("messages.on079addexp"), "messages.on079addexp", variables);
		}
		*/

		/*
		public void On079LevelUp(Player079LevelUpEvent ev)
		{
			Dictionary<string, string> variables = new Dictionary<string, string>
			{
				{ "ipaddress",                  ev.Player.IPAddress                 },
				{ "name",                       ev.Player.Name                      },
				{ "playerid",                   ev.Player.PlayerID.ToString()       },
				{ "steamid",                    ev.Player.GetParsedUserID() ?? ev.Player.UserID },
				{ "class",                      ev.Player.Role.ToString()  },
				{ "team",                       ev.Player.ReferenceHub.GetTeam().ToString()  }
			};
			plugin.SendMessage(Config.GetArray("messages.on079levelup"), "messages.on079levelup", variables);
		}
		*/

		/*
		public void On079UnlockDoors(Player079UnlockDoorsEvent ev)
		{
			if (ev.Allow)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "ipaddress",                  ev.Player.IPAddress                 },
					{ "name",                       ev.Player.Name                      },
					{ "playerid",                   ev.Player.PlayerID.ToString()       },
					{ "steamid",                    ev.Player.GetParsedUserID() ?? ev.Player.UserID },
					{ "class",                      ev.Player.Role.ToString()    },
					{ "team",                       ev.Player.ReferenceHub.GetTeam().ToString()  }
				};
				plugin.SendMessage(Config.GetArray("messages.on079unlockdoors"), "messages.on079unlockdoors", variables);
			}
		}
		*/

		/*
		public void On079CameraTeleport(Player079CameraTeleportEvent ev)
		{
			if (ev.Allow)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "apdrain",                    ev.APDrain.ToString()               },
					{ "ipaddress",                  ev.Player.IPAddress                 },
					{ "name",                       ev.Player.Name                      },
					{ "playerid",                   ev.Player.PlayerID.ToString()       },
					{ "steamid",                    ev.Player.GetParsedUserID() ?? ev.Player.UserID },
					{ "class",                      ev.Player.Role.ToString()    },
					{ "team",                       ev.Player.ReferenceHub.GetTeam().ToString()  }
				};
				plugin.SendMessage(Config.GetArray("messages.on079camerateleport"), "messages.on079camerateleport", variables);
			}
		}
		*/

		/*
		public void On079StartSpeaker(Player079StartSpeakerEvent ev)
		{
			if (ev.Allow)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "apdrain",                    ev.APDrain.ToString()               },
					{ "room",                       ev.Room.RoomType.ToString()         },
					{ "ipaddress",                  ev.Player.IPAddress                 },
					{ "name",                       ev.Player.Name                      },
					{ "playerid",                   ev.Player.PlayerID.ToString()       },
					{ "steamid",                    ev.Player.GetParsedUserID() ?? ev.Player.UserID },
					{ "class",                      ev.Player.Role.ToString()    },
					{ "team",                       ev.Player.ReferenceHub.GetTeam().ToString()  }
				};
				plugin.SendMessage(Config.GetArray("messages.on079startspeaker"), "messages.on079startspeaker", variables);
			}
		}
		*/

		/*
		public void On079StopSpeaker(Player079StopSpeakerEvent ev)
		{
			if (ev.Allow)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "room",                       ev.Room.RoomType.ToString()         },
					{ "ipaddress",                  ev.Player.IPAddress                 },
					{ "name",                       ev.Player.Name                      },
					{ "playerid",                   ev.Player.PlayerID.ToString()       },
					{ "steamid",                    ev.Player.GetParsedUserID() ?? ev.Player.UserID },
					{ "class",                      ev.Player.Role.ToString()  },
					{ "team",                       ev.Player.ReferenceHub.GetTeam().ToString()  }
				};
				plugin.SendMessage(Config.GetArray("messages.on079stopspeaker"), "messages.on079stopspeaker", variables);
			}
		}
		*/

		/*
		public void On079Lockdown(Player079LockdownEvent ev)
		{
			if (ev.Allow)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "apdrain",                    ev.APDrain.ToString()               },
					{ "room",                       ev.Room.RoomType.ToString()         },
					{ "ipaddress",                  ev.Player.IPAddress                 },
					{ "name",                       ev.Player.Name                      },
					{ "playerid",                   ev.Player.PlayerID.ToString()       },
					{ "steamid",                    ev.Player.GetParsedUserID() ?? ev.Player.UserID },
					{ "class",                      ev.Player.Role.ToString()  },
					{ "team",                       ev.Player.ReferenceHub.GetTeam().ToString()  }
				};
				plugin.SendMessage(Config.GetArray("messages.on079lockdown"), "messages.on079lockdown", variables);
			}
		}
		*/

		/*
		public void On079ElevatorTeleport(Player079ElevatorTeleportEvent ev)
		{
			if (ev.Allow)
			{
				Dictionary<string, string> variables = new Dictionary<string, string>
				{
					{ "apdrain",                    ev.APDrain.ToString()               },
					{ "elevator",                   ev.Elevator.ElevatorType.ToString() },
					{ "ipaddress",                  ev.Player.IPAddress                 },
					{ "name",                       ev.Player.Name                      },
					{ "playerid",                   ev.Player.PlayerID.ToString()       },
					{ "steamid",                    ev.Player.GetParsedUserID() ?? ev.Player.UserID },
					{ "class",                      ev.Player.Role.ToString()    },
					{ "team",                       ev.Player.ReferenceHub.GetTeam().ToString()  }
				};
				plugin.SendMessage(Config.GetArray("messages.on079elevatorteleport"), "messages.on079elevatorteleport", variables);
			}
		}
		*/
	}
}