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
         * !TEMP! 20 = hurt
		 * 20 = death
		 */
/*
walking
hurt
sprinting
attacking
attackPrep
knockback
dodging
backpack
interacting
//stealth

dead
quick = knockback
dodge = dodging
aiming = attackPrep
shooting = attacking
press da button = interaction
//sneaky sneaky = stealth 
idle = !walking && !sprinting
*/


if (playerHealth.isDead) 									//***dead***//
{
	animPlayer.SetFloat ("Action", 10f);
}
else // Player is not dead
{
    if (hurt)
    {
        animPlayer.SetFloat ("Action", 20f);
    }
    else
    {
		if (dodging && facingRight) 				/***dodging & right***/
		{
			if(h =< 0)
			{
				animPlayer.SetFloat ("Action", 13f);
                moveSpeed = 6f;
			}
			else if (h > 0)
			{
				animPlayer.SetFloat ("Action", 12f);
                moveSpeed = 6f;
			}
		}
		else if (dodging && !facingRight) 			/***dodging & left***/
		{
			if(h < 0)
			{
				animPlayer.SetFloat ("Action", 12f);
                moveSpeed = 6f;
			}
			else if (h >= 0)
			{
				animPlayer.SetFloat ("Action", 13f);
                moveSpeed = 6f;
			}
		}
		else
        {
            if (knockback) 									/***knockback***/
			{
			animPlayer.SetFloat ("Action", 4f);
			}
			else
			{
				if (aiming && idle) 					/***aiming & not moving***/
				{
					animPlayer.SetFloat ("Action", 1f);
                    moveSpeed = 2f;
                    if (attacking)
						animPlayer.SetFloat ("Action", 2f); //idle aiming & attacking
				}
				else if (aiming && facingRight) 		/***aiming & moving right***/
				{
					if(h < 0)
					{
						animPlayer.SetFloat ("Action", 10f);
                        moveSpeed = 2f;
                        if (attacking)
                            animPlayer.SetFloat ("Action", 11f); //aiming while moving left & attacking (backward)
					}
					else if (h > 0)
					{
						animPlayer.SetFloat ("Action", 7f);
                        moveSpeed = 2f;
                        if (attacking)
                            animPlayer.SetFloat ("Action", 8f); //aiming while moving right & attacking (forward)
					}
				}
				else if (aiming && !facingRight) 		/***aiming & moving left***/
				{
					if(h < 0)
					{
						animPlayer.SetFloat ("Action", 7f);
                        moveSpeed = 2f;
                        if (attacking)
                            animPlayer.SetFloat ("Action", 8f); //aiming while moving left & attacking (forward)
					}
					else if (h > 0)
					{
						animPlayer.SetFloat ("Action", 10f);
                        moveSpeed = 2f;
                        if (attacking)
                            animPlayer.SetFloat ("Action", 11f); //aiming while moving right & attacking (backward)
					}
				}
				else
				{
                    if (running && facingRight && h > 0)
                    {
                        animPlayer.SetFloat ("Action", 5f);
                        moveSpeed = 5f;
                    }
                    else if (running && !facingRight && h < 0)
                    {
                        animPlayer.SetFloat ("Action", 5f);
                        moveSpeed = 5f;
                    }
                    else
                    {   
						if (walking && facingRight && h > 0)
						{
							animPlayer.SetFloat ("Action", 6f);
                            moveSpeed = 3f;
						}
                        else if (walking && !facingRight && h < 0)
                        {
                            animPlayer.SetFloat ("Action", 6f);
                            moveSpeed = 3f;
                        }
						else if (walking && !facingRight && h > 0)
						{
							animPlayer.SetFloat ("Action", 9f);
                            moveSpeed = 3f;
						}
                        else if (walking && facingRight && h < 0)
                        {
                            animPlayer.SetFloat ("Action", 9f);
                            moveSpeed = 3f;
                        }
                        else 
                        {
                            if (interacting)
                            {
                                animPlayer.SetFloat ("Action", 9f);
                            }
                            else
                            {
                                if (idle)
                                {
                                    animPlayer.SetFloat ("Action", 0f);
                                }
                            }
                        }
                    }
				}
			}
		}
    }
}
