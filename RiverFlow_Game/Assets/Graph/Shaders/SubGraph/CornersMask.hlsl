void CornersMask_float( in float2 uv, in uint4 bitmask, out float4 mask)
{
	uint right = 1;
	uint left = 2;
	uint top = 4;
	uint down = 8;

	mask = float4(1,1,1,1);

	uint tl = top + left;
	uint tr = top + right;
	uint dl = down + left;
	uint dr = down + right;

	//S�pare un pixel en 9 block
	uv = frac(uv);
	uv *= 3;

	//Center
	if (uv.x>=1 && uv.x <=2 || uv.y>=1 && uv.y<=2)
		mask = 0;

	//Corner
	if (uv.y > 2 && uv.x > 2)
		mask = ( (bitmask & tr) != 0 )?0:1;
	if (uv.y > 2 && uv.x < 1)
		mask = ( (bitmask & tl) != 0 )?0:1;
	if (uv.y < 1 && uv.x > 2)
		mask = ( (bitmask & dr) != 0 )?0:1;
	if (uv.y < 1 && uv.x < 1)
		mask = ( (bitmask & dl) != 0 )?0:1;
}