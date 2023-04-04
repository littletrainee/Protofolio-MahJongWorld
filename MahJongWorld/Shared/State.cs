namespace MahJongWorld.Shared
{
	public enum State
	{
		Draw,
		CheckTsumo,
		IsTsumo,
		AskDeclareTenPai,
		CheckTenPai,
		Discard,
		CheckRon,
		IsRon,
		CheckMeld,
		MakeTriple,
		MakeBigQuadruple,
		MakeSequence,
		MakeSmallQuadruple,
		MakeConcealedQuadruple,
		CheckConcealedQuadruple,
		DrawFromWall
	}
}
