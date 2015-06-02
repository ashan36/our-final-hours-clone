/* Action number code
		 * 0 = idle
		 * 1 = idle aiming
		 * 2 = idle shooting
		 * 3 = idle interaction
		 * 4 = knockback
		 * 5 = running
		 * 6 = walk forward
		 * 7 = walk aiming forward
		 * 8 = walk shooting forwad
		 * 9 = walking backward
		 * 10 = walk aiming backwards
		 * 11 = walk shooting backwards
		 * 12 = dodge forward
		 * 13 = dodge backwards
		 * 20 = death
		 */

walking
sprinting
attacking
attackPrep
knockback
dodging
backpack
interaction
//stealth

dead
quick = knockback
dodge = dodging
aiming = attackPrep
shooting = attacking
press da button = interaction
//sneaky sneaky = stealth 
idle = !walking && !sprinting


if (attacking && rangedWeapon)
	rangedAnims();

if (playerHealth.isDead) 									//***dead***//
		{
			animPlayer.SetFloat ("Action", 10f);
		}
		else // Player is not dead
		{
			if (knockback) 									/***knockback***/
			{
				animPlayer.SetFloat ("Action", 4f);
			}
			else
			{
				if (dodging && facingRight) 				/***dodging & right***/
				{
					if(h =< 0)
					{
						animPlayer.SetFloat ("Action", 13f);
					}
					else if (h > 0)
					{
						animPlayer.SetFloat ("Action", 12f);
					}
				}
				else if (dodging && !facingRight) 			/***dodging & left***/
				{
					if(h < 0)
					{
						animPlayer.SetFloat ("Action", 12f);
					}
					else if (h >= 0)
					{
						animPlayer.SetFloat ("Action", 13f);
					}
				}
				else
				{
					if (aiming && idle) 					/***aiming & not moving***/
					{
						animPlayer.SetFloat ("Action", 1f);
					}
					else if (aiming && facingRight) 		/***aiming & moving right***/
					{
						if(h < 0)
						{
							animPlayer.SetFloat ("Action", 10f);
						}
						else if (h >= 0)
						{
							animPlayer.SetFloat ("Action", 7f);
						}
					}
					else if (aiming && !facingRight) 		/***aiming & moving left***/
					{
						if(h =< 0)
						{
							animPlayer.SetFloat ("Action", 7f);
						}
						else if (h > 0)
						{
							animPlayer.SetFloat ("Action", 10f);
						}
					}
					else
					{
						if (attacking && idle) 				/***shooting & not moving***/
						{
							animPlayer.SetFloat ("Action", 2f);
						}
						else if (attacking && facingRight) 	/***shooting & moving right***/
						{
							if(h < 0)
							{
								animPlayer.SetFloat ("Action", 11f);
							}
							else if (h >= 0)
							{
								animPlayer.SetFloat ("Action", 8f);
							}
						}
						else if (attacking && !facingRight) 	/***shooting & moving left***/
						{
							if(h =< 0)
							{
								animPlayer.SetFloat ("Action", 8f);
							}
							else if (h > 0)
							{
								animPlayer.SetFloat ("Action", 11f);
							}
						}
						else
						{
							if (walking)
							{
								animPlayer.SetFloat ("Action", 5f);
							}
							else if(!walking)
							{
								animPlayer.SetFloat ("Action", 0f);
							}
							if (attacking)
							{
								animPlayer.SetFloat ("Action", 7f);
							}
							else if (!attacking && !walking)
							{
								animPlayer.SetFloat ("Action", 0f);
							}
							else if (!attacking && walking)
							{
								animPlayer.SetFloat ("Action", 5f);
							}
						}
					}
				}
			}
		}