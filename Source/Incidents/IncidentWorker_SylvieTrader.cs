#nullable enable
using RimWorld;
using RimWorld.Planet;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace SylvieMod;

/// <summary>
/// Incident worker for Sylvie's arrival event.
/// </summary>
public class IncidentWorker_SylvieTrader : IncidentWorker_TraderCaravanArrival
{
    #region Incident Worker Implementation

    /// <summary>
    /// Checks if the incident can fire now.
    /// </summary>
    protected override bool CanFireNowSub(IncidentParms parms)
    {
        if (Current.Game.GetComponent<SylvieGameComponent>().hasSylvieSpawned)
            return false;

        Map target = (Map)parms.target;

        // 使用 SylvieFactionValidator 检查是否存在有效派系
        if (!SylvieFactionValidator.HasAnyValidFaction())
            return false;

        return base.CanFireNowSub(parms) && target.IsPlayerHome;
    }

    /// <summary>
    /// Executes the incident worker.
    /// 修复：先生成希尔薇，成功后再生成商队，避免商队疯狂触发但希尔薇不生成的问题。
    /// </summary>
    protected override bool TryExecuteWorker(IncidentParms parms)
    {
        // 使用 SylvieFactionValidator 获取有效派系
        Faction? validFaction = SylvieFactionValidator.GetValidFaction();
        if (validFaction == null)
            return false;

        parms.faction = validFaction;

        // Set trader kind if not provided
        if (parms.traderKind == null)
        {
            if (parms.faction.def.caravanTraderKinds.NullOrEmpty())
                return false;
            parms.traderKind = parms.faction.def.caravanTraderKinds.RandomElement();
        }

        // 步骤1：先尝试生成希尔薇
        // 这样可以确保希尔薇能成功生成后再生成商队，避免商队来了但希尔薇没生成的问题
        Pawn? sylvie = SylviePawnGenerator.GenerateSylvie(parms.faction);
        if (sylvie == null)
        {
            Log.Warning("[SylvieMod] Failed to generate Sylvie pawn, aborting incident");
            return false;
        }

        // 步骤2：希尔薇生成成功，现在生成商队
        if (!base.TryExecuteWorker(parms))
        {
            // 商队生成失败，销毁已生成的希尔薇
            Log.Warning("[SylvieMod] Failed to spawn trader caravan, destroying generated Sylvie");
            sylvie.Destroy();
            return false;
        }

        // 步骤3：商队生成成功，找到 trader 并将希尔薇放到其旁边
        Map target = (Map)parms.target;
        List<Pawn> traders = target.mapPawns.AllPawnsSpawned
            .Where(p => p.trader != null && p.Faction == parms.faction)
            .ToList();

        if (traders.NullOrEmpty())
        {
            // 没有找到 trader，销毁希尔薇
            Log.Warning("[SylvieMod] No trader found in caravan, destroying generated Sylvie");
            sylvie.Destroy();
            return false;
        }

        // 步骤4：将希尔薇放置到 trader 旁边并发送信件
        SpawnSylvieAndSendOffer(traders.RandomElement(), sylvie, target);
        return true;
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Spawns Sylvie and sends the offer letter.
    /// </summary>
    private void SpawnSylvieAndSendOffer(Pawn traderLeader, Pawn sylvie, Map map)
    {
        GenSpawn.Spawn(sylvie, traderLeader.Position, map);
        sylvie.guest.SetGuestStatus(traderLeader.Faction, GuestStatus.Prisoner);
        traderLeader.lord?.AddPawn(sylvie);

        Current.Game.GetComponent<SylvieGameComponent>().hasSylvieSpawned = true;
        Current.Game.GetComponent<SylvieGameComponent>().RegisterSylviePawn(sylvie);

        SendOfferLetter(traderLeader, sylvie, map);
    }

    /// <summary>
    /// Sends the offer letter to the player.
    /// </summary>
    private void SendOfferLetter(Pawn trader, Pawn sylvie, Map map)
    {
        LetterDef? letterDef = SylvieDefNames.Letter_OfferLetterDef;
        if (letterDef == null)
        {
            Log.Error("[SylvieMod] Could not find Sylvie_OfferLetter LetterDef");
            return;
        }

        // 使用 LetterMaker 创建 Letter，确保正确分配 Load ID
        ChoiceLetter_SylvieOffer letter = (ChoiceLetter_SylvieOffer)LetterMaker.MakeLetter(letterDef);
        letter.Configure(trader, sylvie, map);
        Find.LetterStack.ReceiveLetter(letter);
    }

    #endregion
}
