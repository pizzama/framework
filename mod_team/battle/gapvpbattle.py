import copy
import math
from blueprint.blueprints.build import Builds
from blueprint.blueprints.guild import Guild
from blueprint.blueprints.role import Role
from blueprint.blueprints.technology import PersonalTech
from game.exceptions import GameException
from game.core.mods.mod_team.battle.battle import Battle
from game.models.dbguild import DbGuildManager
from game.models.dbrole import DbRoleManager
from game.servers.serverstatic import AIMTYPE
from game.core.mods.mod_team.battle.killinfo import KillInfos


class PvpScetion:
    def __init__(self, pvp, battle_type):
        self.pvp = pvp
        self.battle_type = battle_type
        self.is_init = False
        self.last_battle_effects = None

    def init(self):
        pass

    def update(self):
        pass

    @staticmethod
    def get_hp_buff(battle_effect_list, country, battle_type):
        hp_buff = [0, 0, 0]
        for buff in battle_effect_list:
            if buff[2] == 12 and buff[3] in (0, country):
                hp_buff[1] += buff[4]
            elif buff[2] == 46 and buff[3] in (0, country):
                hp_buff[0] += buff[4]
            elif buff[2] == 47 and buff[3] in (0, country):
                hp_buff[2] += buff[4]
            elif buff[2] == 50 and buff[3] in (0, country):
                hp_buff[1] += buff[4]
            elif buff[2] == 78 and battle_type in (AIMTYPE.TEMPOSEDEFEN, AIMTYPE.GUARDBLOCK) and buff[3] in (
                    0, country):
                hp_buff[1] += buff[4]
        return hp_buff

    @staticmethod
    def get_attack_buff(battle_effect_list, country, battle_type):
        attack_buff = [0, 0, 0]
        for buff in battle_effect_list:
            if buff[2] == 48 and buff[3] in (0, country):
                attack_buff[2] += buff[4]
            elif buff[2] == 51 and buff[3] in (0, country):
                attack_buff[1] += buff[4]
            elif buff[2] == 69 and battle_type in (
                    AIMTYPE.TEMPOSE, AIMTYPE.GUILDBLOCK, AIMTYPE.SEAL, AIMTYPE.WILDBLOCK) and buff[3] in (0, country):
                attack_buff[1] += buff[4]
            elif buff[2] == 72 and battle_type in (AIMTYPE.TEMPOSE, AIMTYPE.GUILDBLOCK) and buff[3] in (0, country):
                attack_buff[1] += buff[4]
            elif buff[2] == 76 and battle_type in (AIMTYPE.TEMPOSEDEFEN, AIMTYPE.GUARDBLOCK) and buff[3] in (0, country):
                attack_buff[1] += buff[4]
        return attack_buff

    @staticmethod
    def get_defen_buff(battle_effect_list, country, battle_type):
        defen_buff = [0, 0, 0]
        for buff in battle_effect_list:
            if buff[2] == 49 and buff[3] in (0, country):
                defen_buff[2] += buff[4]
            elif buff[2] == 52 and buff[3] in (0, country):
                defen_buff[1] += buff[4]
            elif buff[2] == 80 and battle_type in (
                    AIMTYPE.TEMPOSE, AIMTYPE.GUILDBLOCK, AIMTYPE.SEAL, AIMTYPE.WILDBLOCK) and buff[3] in (0, country):
                defen_buff[1] += buff[4]
        return defen_buff

    @staticmethod
    def get_effectshit_buff(battle_effect_list, country):
        effectshit_buff = [0, 0]
        for buff in battle_effect_list:
            if buff[2] == 63 and buff[3] in (0, country):
                effectshit_buff[0] += buff[4]
        return effectshit_buff

    @staticmethod
    def get_effectsdef_buff(battle_effect_list, country):
        effectsdef_buff = [0, 0]
        for buff in battle_effect_list:
            if buff[2] == 64 and buff[3] in (0, country):
                effectsdef_buff[0] += buff[4]
        return effectsdef_buff

    @staticmethod
    def get_speed_buff(battle_effect_list, country):
        speed_buff = [0, 0]
        for buff in battle_effect_list:
            if buff[2] == 65 and buff[3] in (0, country):
                speed_buff[1] += buff[4]
        return speed_buff

    @staticmethod
    def get_battle_damage_buff(battle_effect_list, battle_type):
        damage_buff = {1: [0, 0], 2: [0, 0], 3: [0, 0], 4: [0, 0]}
        for buff in battle_effect_list:
            if buff[2] == 66:
                if buff[3] == 1:
                    damage_buff[1][0] += buff[4]
                elif buff[3] == 2:
                    damage_buff[2][0] += buff[4]
                elif buff[3] == 3:
                    damage_buff[3][0] += buff[4]
                elif buff[3] == 4:
                    damage_buff[4][0] += buff[4]
                elif buff[3] == 0:
                    damage_buff[1][0] += buff[4]
                    damage_buff[2][0] += buff[4]
                    damage_buff[3][0] += buff[4]
                    damage_buff[4][0] += buff[4]
            elif buff[2] == 67:
                if buff[3] == 1:
                    damage_buff[1][1] += buff[4]
                elif buff[3] == 2:
                    damage_buff[2][1] += buff[4]
                elif buff[3] == 3:
                    damage_buff[3][1] += buff[4]
                elif buff[3] == 4:
                    damage_buff[4][1] += buff[4]
                elif buff[3] == 0:
                    damage_buff[1][1] += buff[4]
                    damage_buff[2][1] += buff[4]
                    damage_buff[3][1] += buff[4]
                    damage_buff[4][1] += buff[4]
            elif buff[2] == 75 and battle_type in (AIMTYPE.TEMPOSE, AIMTYPE.GUILDBLOCK):
                if buff[3] == 1:
                    damage_buff[1][1] += buff[4]
                elif buff[3] == 2:
                    damage_buff[2][1] += buff[4]
                elif buff[3] == 3:
                    damage_buff[3][1] += buff[4]
                elif buff[3] == 4:
                    damage_buff[4][1] += buff[4]
                elif buff[3] == 0:
                    damage_buff[1][1] += buff[4]
                    damage_buff[2][1] += buff[4]
                    damage_buff[3][1] += buff[4]
                    damage_buff[4][1] += buff[4]
            elif buff[2] == 79 and battle_type in (AIMTYPE.TEMPOSEDEFEN, AIMTYPE.GUARDBLOCK):
                if buff[3] == 1:
                    damage_buff[1][1] += buff[4]
                elif buff[3] == 2:
                    damage_buff[2][1] += buff[4]
                elif buff[3] == 3:
                    damage_buff[3][1] += buff[4]
                elif buff[3] == 4:
                    damage_buff[4][1] += buff[4]
                elif buff[3] == 0:
                    damage_buff[1][1] += buff[4]
                    damage_buff[2][1] += buff[4]
                    damage_buff[3][1] += buff[4]
                    damage_buff[4][1] += buff[4]
        return damage_buff

    @staticmethod
    def get_resist_damage_buff(battle_effect_list, battle_type):
        resist_damage_buff = {1: [0, 0], 2: [0, 0], 3: [0, 0], 4: [0, 0]}
        for buff in battle_effect_list:
            if buff[2] == 73 and battle_type in (AIMTYPE.TEMPOSEDEFEN, AIMTYPE.GUARDBLOCK):
                if buff[3] == 1:
                    resist_damage_buff[1][1] += buff[4]
                elif buff[3] == 2:
                    resist_damage_buff[2][1] += buff[4]
                elif buff[3] == 3:
                    resist_damage_buff[3][1] += buff[4]
                elif buff[3] == 4:
                    resist_damage_buff[4][1] += buff[4]
                elif buff[3] == 0:
                    resist_damage_buff[1][1] += buff[4]
                    resist_damage_buff[2][1] += buff[4]
                    resist_damage_buff[3][1] += buff[4]
                    resist_damage_buff[4][1] += buff[4]
            elif buff[2] == 77 and battle_type in (AIMTYPE.TEMPOSE, AIMTYPE.GUILDBLOCK):
                if buff[3] == 1:
                    resist_damage_buff[1][1] += buff[4]
                elif buff[3] == 2:
                    resist_damage_buff[2][1] += buff[4]
                elif buff[3] == 3:
                    resist_damage_buff[3][1] += buff[4]
                elif buff[3] == 4:
                    resist_damage_buff[4][1] += buff[4]
                elif buff[3] == 0:
                    resist_damage_buff[1][1] += buff[4]
                    resist_damage_buff[2][1] += buff[4]
                    resist_damage_buff[3][1] += buff[4]
                    resist_damage_buff[4][1] += buff[4]
        return resist_damage_buff

    @staticmethod
    def get_exp_buff(battle_effect_list):
        exp_buff = [0, 0]
        for buff in battle_effect_list:
            if buff[2] == 33:
                exp_buff[1] += buff[4]
        return exp_buff

    async def async_add_card_buff(self, card, battle_effect_list, battle_type):
        country = card.country
        hp_buff = self.get_hp_buff(battle_effect_list, country, battle_type)
        card.hp += hp_buff[2]
        card.hp *= (100 + hp_buff[1]) / 100
        card.hp += hp_buff[0]
        card.maxhp = card.hp
        card.recordhp = card.hp
        attack_buff = self.get_attack_buff(battle_effect_list, country, battle_type)
        card.attack += attack_buff[2]
        card.attack *= (100 + attack_buff[1]) / 100
        card.attack += attack_buff[0]
        defen_buff = self.get_defen_buff(battle_effect_list, country, battle_type)
        card.defen += defen_buff[2]
        card.defen *= (100 + defen_buff[1]) / 100
        card.defen += defen_buff[0]
        speed_buff = self.get_speed_buff(battle_effect_list, country)
        card.speed *= (100 + speed_buff[1]) / 100
        effectshit_buff = self.get_effectshit_buff(battle_effect_list, country)
        card.effectshit += effectshit_buff[0]
        effectsdef_buff = self.get_effectsdef_buff(battle_effect_list, country)
        card.effectsdef += effectsdef_buff[0]

    @staticmethod
    def get_buff_record(battle_effect_list):
        buff_record = {
            "hp_buff": [0, 0, 0, 0],
            "attack_buff": [0, 0, 0, 0],
            "defense_buff": [0, 0, 0, 0],
        }
        for buff in battle_effect_list:
            if buff[2] == 50:
                if buff[3] == 1:
                    buff_record["hp_buff"][0] += buff[4]
                elif buff[3] == 2:
                    buff_record["hp_buff"][1] += buff[4]
                elif buff[3] == 3:
                    buff_record["hp_buff"][2] += buff[4]
                elif buff[3] == 4:
                    buff_record["hp_buff"][3] += buff[4]
                elif buff[3] == 0:
                    buff_record["hp_buff"][0] += buff[4]
                    buff_record["hp_buff"][1] += buff[4]
                    buff_record["hp_buff"][2] += buff[4]
                    buff_record["hp_buff"][3] += buff[4]
            elif buff[2] == 51:
                if buff[3] == 1:
                    buff_record["attack_buff"][0] += buff[4]
                elif buff[3] == 2:
                    buff_record["attack_buff"][1] += buff[4]
                elif buff[3] == 3:
                    buff_record["attack_buff"][2] += buff[4]
                elif buff[3] == 4:
                    buff_record["attack_buff"][3] += buff[4]
                elif buff[3] == 0:
                    buff_record["attack_buff"][0] += buff[4]
                    buff_record["attack_buff"][1] += buff[4]
                    buff_record["attack_buff"][2] += buff[4]
                    buff_record["attack_buff"][3] += buff[4]
            elif buff[2] == 52:
                if buff[3] == 1:
                    buff_record["defense_buff"][0] += buff[4]
                elif buff[3] == 2:
                    buff_record["defense_buff"][1] += buff[4]
                elif buff[3] == 3:
                    buff_record["defense_buff"][2] += buff[4]
                elif buff[3] == 4:
                    buff_record["defense_buff"][3] += buff[4]
                elif buff[3] == 0:
                    buff_record["defense_buff"][0] += buff[4]
                    buff_record["defense_buff"][1] += buff[4]
                    buff_record["defense_buff"][2] += buff[4]
                    buff_record["defense_buff"][3] += buff[4]

        return buff_record


class FirstSection(PvpScetion):
    def init(self):
        self.is_init = True
        self.pvp.curstage = 1
        log = {}
        log["stage"] = self.pvp.curstage
        return {"win": 2, "log": log, "round": self.pvp.curround}

    async def handle(self, attackteams, defenseteams):
        effect = self.caculate_loco(attackteams)
        atk = attackteams[0]
        # 复制车头，临时设置属性
        arr = []
        for cha in atk:
            arr.append(cha.deepcopy())
        atk = arr
        atkleader = self.get_leader(atk)
        atk_role = atkleader.role
        db_role = await DbRoleManager().get_async_db_role(atk_role.role_id)
        builds = Builds(db_model=db_role)
        await builds.async_read()
        personaltech = PersonalTech(db_model=db_role)
        await personaltech.async_read()
        proxy = [builds, personaltech]
        if atk_role.guild_id:
            db_guild = await DbGuildManager().get_async_db_guild(atk_role.guild_id)
            guild = Guild(db_model=db_guild)
            await guild.async_read()
            proxy.append(guild)
        atk_role.add_blueprints(proxy)
        atk_battle_effect_list = await atk_role.get_async_buff_effect()
        atk = self.merge_loco(atk, effect)

        effect = self.caculate_loco(defenseteams)
        defense = defenseteams[0]
        # 复制车头，临时设置属性
        arr = []
        for cha in defense:
            arr.append(cha.deepcopy())
        defense = arr
        defleader = self.get_leader(defense)
        defense_role = defleader.role
        db_role = await DbRoleManager().get_async_db_role(defense_role.role_id)
        builds = Builds(db_model=db_role)
        await builds.async_read()
        personaltech = PersonalTech(db_model=db_role)
        await personaltech.async_read()
        proxy = [builds, personaltech]
        if defense_role.guild_id:
            db_guild = await DbGuildManager().get_async_db_guild(defense_role.guild_id)
            guild = Guild(db_model=db_guild)
            await guild.async_read()
            proxy.append(guild)
        defense_role.add_blueprints(proxy)
        defen_battle_effect_list = await defense_role.get_async_buff_effect()
        defense = self.merge_loco(defense, effect)

        battle = Battle(maxround=30, battleplace=1)
        akt_cards = battle.create_side_cards(atk, 1)
        battle_type = self.battle_type
        for card in akt_cards:
            character = card.character
            team_buff_list = character.get_team_buff_effect()
            atk_battle_effect_list.extend(team_buff_list)
        for card in akt_cards:
            character_buff_list = card.character.get_character_buff_effect()
            atk_battle_effects = atk_battle_effect_list.copy() + character_buff_list
            await self.async_add_card_buff(card, atk_battle_effects, battle_type)
            card.damage_buff = self.get_battle_damage_buff(atk_battle_effects, battle_type)
            card.resist_damage_buff = self.get_resist_damage_buff(atk_battle_effects, battle_type)
            card.exp_buff = self.get_exp_buff(atk_battle_effects)

        def_cards = battle.create_side_cards(defense, -1)
        for card in def_cards:
            character = card.character
            team_buff_list = character.get_team_buff_effect()
            defen_battle_effect_list.extend(team_buff_list)
        defen_type = AIMTYPE.TEMPOSEDEFEN if battle_type == AIMTYPE.TEMPOSE else AIMTYPE.GUARDBLOCK
        for card in def_cards:
            character_buff_list = card.character.get_character_buff_effect()
            defen_battle_effects = defen_battle_effect_list.copy() + character_buff_list
            await self.async_add_card_buff(card, defen_battle_effects, defen_type)
            card.damage_buff = self.get_battle_damage_buff(defen_battle_effects, defen_type)
            card.resist_damage_buff = self.get_resist_damage_buff(defen_battle_effects, defen_type)
            card.exp_buff = self.get_exp_buff(defen_battle_effects)

        battle.team_copy()
        battle.create_team_log()
        self.battle = battle
        # 开始战斗前记录战斗
        atk_buff_list = self.get_buff_record(atk_battle_effect_list)
        def_buff_list = self.get_buff_record(defen_battle_effect_list)
        await self.pvp.add_team_record_log(status="start", win=2,
                                           atkteam=akt_cards, is_atk_leader=1,
                                           defteam=def_cards, is_def_leader=1,
                                           atk_buff_list=atk_buff_list,
                                           def_buff_list=def_buff_list)
        self.pvp.atk_battle_effects = atk_buff_list
        self.pvp.def_battle_effects = def_buff_list
        self.pvp.atk_leader_cards = akt_cards
        self.pvp.def_leader_cards = def_cards

    async def update(self):
        if not self.is_init:
            return self.init()
        if not self.battle or not self.pvp:
            return
        self.battle.battlelog.clean()
        rt = self.battle.round_single_go()
        if rt != 2:  # 如果有战斗结果了则开始计算疲劳值
            # 获得疲劳值，平均给每一个人
            atkfatigue, deffatigue, _, _ = self.pvp.caculate_team_fatigue_vule(self.battle.team_backup(), rt)
            stratum, rids, atk_fatigue_record = self.pvp.average_fatigue(
                characters=self.pvp.atkbackup,
                fatigue=atkfatigue,
                basevalue=100,
                win=rt)
            self.pvp.kill.update_percent_kill_except_roleids(rids, stratum)
            stratum, rids, def_fatigue_record = self.pvp.average_fatigue(
                characters=self.pvp.defenbackup,
                fatigue=deffatigue,
                basevalue=100,
                win=1 - rt)
            self.pvp.kill.update_percent_kill_except_roleids(rids, stratum)

        log = self.battle.caclute_team_hp(self.pvp)
        self.pvp.curround += 1
        atkfatigue, deffatigue = self.pvp.record_current_fatigue_vule()
        log["atk_fatigue"] = atkfatigue
        log["def_fatigue"] = deffatigue
        if rt != 2:
            # 处理战斗残留数据
            team, side = self.battle.copy_win_side()
            self.pvp.check_teams()  # 检查武将精力值
            rt = self.pvp.reset_teams(side)
            print("FIRST SECTION FINISH:", rt)
            if rt != 2:
                if rt == 0:
                    self.pvp.first_section_win = 1
                elif rt == 1:
                    self.pvp.first_section_win = -1
                # 车头战斗结束记录互相的killinfo
                # self.pvp.update_side_kill(1, deffatigue)
                # self.pvp.update_side_kill(-1, atkfatigue)
                raise GameException(status=185, msg={"win": rt, "log": log, "round": self.pvp.curround})

            await self.pvp.add_team_record_log(status="end", win=rt,
                                               atkteam=self.pvp.atk_leader_cards, is_atk_leader=1,
                                               defteam=self.pvp.def_leader_cards, is_def_leader=1,
                                               atk_fatigue_record=atk_fatigue_record,
                                               def_fatigue_record=def_fatigue_record)
            self.battle.record_team_hp()
            section = SecondSection(self.pvp, self.battle_type)
            section.handle(team, side)
            if rt == 0:
                await self.pvp.add_team_record_log(status="start", win=2,
                                                   atkteam=self.pvp.atk_leader_cards, is_atk_leader=1,
                                                   defteam=self.pvp.def_leader_cards, is_def_leader=0,
                                                   atk_buff_list=self.pvp.atk_battle_effects,
                                                   def_buff_list=self.pvp.def_battle_effects)
            else:
                await self.pvp.add_team_record_log(status="start", win=2,
                                                   atkteam=self.pvp.atk_leader_cards, is_atk_leader=0,
                                                   defteam=self.pvp.def_leader_cards, is_def_leader=1,
                                                   atk_buff_list=self.pvp.atk_battle_effects,
                                                   def_buff_list=self.pvp.def_battle_effects)
            self.pvp.change_section(section)

        return {"win": rt, "log": log, "round": self.pvp.curround}

    def caculate_loco(self, characters):
        hp = 0
        atk = 0
        defend = 0
        count = 0
        for team in characters:
            for cha in team:
                effects = cha.effect  # effect是临时属性
                count += 1  # 记录所有武将个数不论有没有效果
                hp += cha.propertys.hp
                for ect in effects:
                    if ect[2] == 47:
                        hp += ect[4]
                    elif ect[3] == 48:
                        atk += ect[4]
                    elif ect[4] == 49:
                        defend += ect[4]
        return [hp, atk, defend, count]

    def merge_loco(self, chas, arr):
        result = []
        for cha in chas:
            count = arr[3]
            cha.propertys.hp += arr[0] / 3
            cha.propertys.atk += arr[1] / count
            cha.propertys.defend += arr[2] / count
            result.append(cha)
        return result

    def get_leader(self, chas):
        for cha in chas:
            if cha.is_leader:
                return cha
        return None


class SecondSection(PvpScetion):
    def init(self):
        self.is_init = True
        self.pvp.curstage = 2
        self.person_fatigue_record = {}
        self.average_fatigue_record = {}
        log = {}
        log["stage"] = self.pvp.curstage
        self.pvp.curround += 1
        return {"win": 2, "log": log, "round": self.pvp.curround}

    def handle(self, team, side):
        print("Second SECTION start:", side)
        self.side = side
        self.teamleader = team
        if side == 1:  # 上一阶段胜利的是atk
            self.otherteams = self.pvp.defenseteams
            self.last_battle_effects = self.pvp.atk_battle_effects
        elif side == -1:  # 上一阶段胜利的是def
            self.otherteams = self.pvp.atkteams
            self.last_battle_effects = self.pvp.def_battle_effects

    async def leader_person_battle(self):
        battle = Battle(maxround=30, battleplace=1)
        defense = self.otherteams.pop(0)
        def_cards = battle.create_side_cards(defense, -self.side)
        print("leader_person_battle:", defense)
        defense_role = defense[0].role
        db_role = await DbRoleManager().get_async_db_role(defense_role.role_id)
        builds = Builds(db_model=db_role)
        await builds.async_read()
        personaltech = PersonalTech(db_model=db_role)
        await personaltech.async_read()
        proxy = [builds, personaltech]
        if defense_role.guild_id:
            db_guild = await DbGuildManager().get_async_db_guild(defense_role.guild_id)
            guild = Guild(db_model=db_guild)
            await guild.async_read()
            proxy.append(guild)
        defense_role.add_blueprints(proxy)
        defense_role.refresh_role_buff()
        battle_effect_list = await defense_role.get_async_buff_effect()
        for card in def_cards:
            character = card.character
            team_buff_list = character.get_team_buff_effect()
            battle_effect_list.extend(team_buff_list)

        battle_type = self.battle_type
        if -self.side == -1:
            if battle_type == AIMTYPE.TEMPOSE:
                battle_type = AIMTYPE.TEMPOSEDEFEN
            else:
                battle_type = AIMTYPE.GUARDBLOCK

        for card in def_cards:
            character = card.character
            character_buff_list = character.get_character_buff_effect()
            battle_effects = battle_effect_list.copy() + character_buff_list
            await self.async_add_card_buff(card, battle_effects, battle_type)
            card.damage_buff = self.get_battle_damage_buff(battle_effects, battle_type)
            card.resist_damage_buff = self.get_resist_damage_buff(battle_effects, battle_type)
            card.exp_buff = self.get_exp_buff(battle_effects)

        self.new_battle_effects = self.get_buff_record(battle_effect_list)
        battle.set_side_cards(self.teamleader, self.side)
        battle.team_copy()
        return battle

    def check_over(self, log):
        self.pvp.check_teams()  # 检查武将精力值
        if len(self.otherteams) == 0:
            rt = self.pvp.reset_teams(self.side)
            if rt != 2:
                raise GameException(status=185, msg={"win": rt, "log": log, "round": self.pvp.curround})
            else:
                if self.side == 1:
                    self.otherteams = self.pvp.defenseteams
                elif self.side == -1:
                    self.otherteams = self.pvp.atkteams

    async def update(self):
        if not self.is_init:
            return self.init()
        self.check_over({})
        battle = await self.leader_person_battle()

        if self.side == 1:  # 上一阶段胜利的是atk, 新上场的是def
            self.pvp.atk_battle_effects = self.last_battle_effects
            self.pvp.def_battle_effects = self.new_battle_effects
        else:  # 上一阶段胜利的是def, 新上场的是atk
            self.pvp.atk_battle_effects = self.new_battle_effects
            self.pvp.def_battle_effects = self.last_battle_effects

        result = battle.start()
        rt = 0 if result else 1

        atkfatigue, deffatigue, atkstratum, defstratum = self.pvp.caculate_team_fatigue_vule(battle.team_backup(), rt)
        # 另一方承担基础伤害
        team = battle.team_backup()
        # 记录不是车头的角色的精力值消耗
        if self.side == 1:  # 上一阶段胜利的是atk, 新上场的是def
            self.pvp.record_card_fatigue(team, -self.side, 1 - rt, self.person_fatigue_record)
        else:
            self.pvp.record_card_fatigue(team, -self.side, rt, self.person_fatigue_record)
        # 车头分摊所有伤害
        if self.side == 1:  # 上一阶段胜利的是atk
            stratum, rids, average_fatigue_record = self.pvp.average_fatigue(
                characters=self.pvp.atkbackup, fatigue=atkfatigue,
                win=rt, average_fatigue_record=self.average_fatigue_record
            )
            self.pvp.kill.update_percent_kill_except_roleids(rids, stratum)
            self.pvp.update_team_kill(team[-self.side], atkstratum)
        elif self.side == -1:
            stratum, rids, average_fatigue_record = self.pvp.average_fatigue(
                characters=self.pvp.defenbackup, fatigue=deffatigue,
                win=1 - rt, average_fatigue_record=self.average_fatigue_record
            )
            self.pvp.kill.update_percent_kill_except_roleids(rids, stratum)
            self.pvp.update_team_kill(team[-self.side], defstratum)

        team, side = battle.copy_win_side()
        if side == self.side:  # 继续进行
            self.teamleader = team
            if side == 1:  # 攻击方胜利，保存攻方buff
                self.last_battle_effects = self.pvp.atk_battle_effects
            else:  # 防守方胜利，保存守防buff
                self.last_battle_effects = self.pvp.def_battle_effects
        else:
            if self.side == 1:  # 上一阶段胜利的是atk, 新上场的是def
                await self.pvp.add_team_record_log(status="end", win=rt,
                                                   atkteam=self.pvp.atk_leader_cards, is_atk_leader=1,
                                                   defteam=self.pvp.def_leader_cards, is_def_leader=0,
                                                   atk_fatigue_record=self.average_fatigue_record,
                                                   def_fatigue_record=self.person_fatigue_record)
            elif self.side == -1:  # 上一阶段胜利的是def, 新上场的是atk
                await self.pvp.add_team_record_log(status="end", win=rt,
                                                   atkteam=self.pvp.atk_leader_cards, is_atk_leader=0,
                                                   defteam=self.pvp.def_leader_cards, is_def_leader=1,
                                                   atk_fatigue_record=self.person_fatigue_record,
                                                   def_fatigue_record=self.average_fatigue_record)
            section = ThirdSeciton(self.pvp, self.battle_type)
            self.pvp.change_section(section)  # 进入第三阶段
            section.handle(team, side)
            if side == 1:  # 攻击方胜利，保存攻方buff
                section.last_battle_effects = self.pvp.atk_battle_effects
            else:  # 防守方胜利，保存守防buff
                section.last_battle_effects = self.pvp.def_battle_effects
            await self.pvp.add_team_record_log(status="start", win=2,
                                               atkteam=self.pvp.atk_leader_cards, is_atk_leader=0,
                                               defteam=self.pvp.def_leader_cards, is_def_leader=0,
                                               atk_buff_list=self.pvp.atk_battle_effects,
                                               def_buff_list=self.pvp.def_battle_effects)

        self.pvp.curround += 1
        log = battle.caclute_team_hp(self.pvp)

        atkfatigue, deffatigue = self.pvp.record_current_fatigue_vule()
        log["atk_fatigue"] = atkfatigue
        log["def_fatigue"] = deffatigue
        # 检查是否还能继续战斗
        self.check_over(log)
        return {"win": 2, "log": log, "round": self.pvp.curround, "ca_time": battle.battlelog.ca_time}


class ThirdSeciton(PvpScetion):
    def init(self):
        self.is_init = True
        self.pvp.curstage = 3
        self.atk_fatigue_record = {}
        self.def_fatigue_record = {}
        log = {}
        log["stage"] = self.pvp.curstage
        self.pvp.curround += 1
        return {"win": 2, "log": log, "round": self.pvp.curround}

    def handle(self, team, side):
        self.team = team
        self.side = side
        self.defenseteams = self.pvp.defenseteams
        self.attackteams = self.pvp.atkteams

    def check_over(self, log):
        self.pvp.check_teams()  # 检查武将精力值
        if len(self.pvp.defenseteams) == 0:
            rt = self.pvp.reset_teams(1)
            if rt != 2:
                raise GameException(status=185, msg={"win": rt, "log": log, "round": self.pvp.curround})
        if len(self.pvp.atkteams) == 0:
            rt = self.pvp.reset_teams(-1)
            if rt != 2:
                raise GameException(status=185, msg={"win": rt, "log": log, "round": self.pvp.curround})
        self.defenseteams = self.pvp.defenseteams
        self.attackteams = self.pvp.atkteams

        if len(self.pvp.defenseteams) == 0 or len(self.pvp.atkteams) == 0:
            return rt
        else:
            return 2

    async def person_person_battle(self):
        arr = []
        if self.team:
            battle = Battle(maxround=30, battleplace=1)
            if self.side == 1:  # 上一阶段胜利的是atk, 新上场的是def
                defens = self.defenseteams.pop(0)
                battle.set_side_cards(self.team, 1)
                def_cards = battle.create_side_cards(defens, -1)

                defens_role = defens[0].role
                db_role = await DbRoleManager().get_async_db_role(defens_role.role_id)
                builds = Builds(db_model=db_role)
                await builds.async_read()
                personaltech = PersonalTech(db_model=db_role)
                await personaltech.async_read()
                proxy = [builds, personaltech]
                if defens_role.guild_id:
                    db_guild = await DbGuildManager().get_async_db_guild(defens_role.guild_id)
                    guild = Guild(db_model=db_guild)
                    await guild.async_read()
                    proxy.append(guild)
                defens_role.add_blueprints(proxy)
                defens_role.refresh_role_buff()
                defens_battle_effect_list = await defens_role.get_async_buff_effect()
                for card in def_cards:
                    character = card.character
                    team_buff_list = character.get_team_buff_effect()
                    defens_battle_effect_list.extend(team_buff_list)

                battle_type = self.battle_type
                if battle_type == AIMTYPE.TEMPOSE:
                    battle_type = AIMTYPE.TEMPOSEDEFEN
                else:
                    battle_type = AIMTYPE.GUARDBLOCK

                for card in def_cards:
                    character_buff_list = card.character.get_character_buff_effect()
                    battle_effects = defens_battle_effect_list.copy() + character_buff_list
                    await self.async_add_card_buff(card, battle_effects, battle_type)
                    card.damage_buff = self.get_battle_damage_buff(battle_effects, battle_type)
                    card.resist_damage_buff = self.get_resist_damage_buff(battle_effects, battle_type)
                    card.exp_buff = self.get_exp_buff(battle_effects)

                battle.team_copy()
                battle.atk_battle_effects = self.last_battle_effects
                battle.def_battle_effects = self.get_buff_record(defens_battle_effect_list)
                arr.append(battle)

            elif self.side == -1:  # 上一阶段胜利的是def, 新上场的是atk
                atks = self.attackteams.pop(0)
                cards = battle.create_side_cards(atks, 1)

                atks_role = atks[0].role
                db_role = await DbRoleManager().get_async_db_role(atks_role.role_id)
                builds = Builds(db_model=db_role)
                await builds.async_read()
                personaltech = PersonalTech(db_model=db_role)
                await personaltech.async_read()
                proxy = [builds, personaltech]
                if atks_role.guild_id:
                    db_guild = await DbGuildManager().get_async_db_guild(atks_role.guild_id)
                    guild = Guild(db_model=db_guild)
                    await guild.async_read()
                    proxy.append(guild)
                atks_role.add_blueprints(proxy)
                atks_role.refresh_role_buff()
                atks_battle_effect_list = await atks_role.get_async_buff_effect()
                for card in cards:
                    character = card.character
                    team_buff_list = character.get_team_buff_effect()
                    atks_battle_effect_list.extend(team_buff_list)

                battle_type = self.battle_type

                for card in cards:
                    character_buff_list = card.character.get_character_buff_effect()
                    battle_effects = atks_battle_effect_list.copy() + character_buff_list
                    await self.async_add_card_buff(card, battle_effects, battle_type)
                    card.damage_buff = self.get_battle_damage_buff(battle_effects, battle_type)
                    card.resist_damage_buff = self.get_resist_damage_buff(battle_effects, battle_type)
                    card.exp_buff = self.get_exp_buff(battle_effects)

                battle.set_side_cards(self.team, -1)
                battle.team_copy()
                battle.atk_battle_effects = self.get_buff_record(atks_battle_effect_list)
                battle.def_battle_effects = self.last_battle_effects
                arr.append(battle)
            self.team = None

        for i in range(2):  # 改成3v3
            if len(self.attackteams) > i and len(self.defenseteams) > i:
                battle = Battle(maxround=30, battleplace=1)
                atks = self.attackteams.pop(0)
                defens = self.defenseteams.pop(0)
                atk_cards = battle.create_side_cards(atks, 1)

                atks_role = atks[0].role
                db_role = await DbRoleManager().get_async_db_role(atks_role.role_id)
                builds = Builds(db_model=db_role)
                await builds.async_read()
                personaltech = PersonalTech(db_model=db_role)
                await personaltech.async_read()
                proxy = [builds, personaltech]
                if atks_role.guild_id:
                    db_guild = await DbGuildManager().get_async_db_guild(atks_role.guild_id)
                    guild = Guild(db_model=db_guild)
                    await guild.async_read()
                    proxy.append(guild)
                atks_role.add_blueprints(proxy)
                atks_role.refresh_role_buff()
                atks_battle_effect_list = await atks_role.get_async_buff_effect()
                battle_type = self.battle_type
                for card in atk_cards:
                    character = card.character
                    team_buff_list = character.get_team_buff_effect()
                    atks_battle_effect_list.extend(team_buff_list)
                for card in atk_cards:
                    character_buff_list = card.character.get_character_buff_effect()
                    atk_battle_effects = atks_battle_effect_list.copy() + character_buff_list
                    await self.async_add_card_buff(card, atk_battle_effects, battle_type)
                    card.damage_buff = self.get_battle_damage_buff(atk_battle_effects, battle_type)
                    card.resist_damage_buff = self.get_resist_damage_buff(atk_battle_effects, battle_type)
                    card.exp_buff = self.get_exp_buff(atk_battle_effects)

                def_cards = battle.create_side_cards(defens, -1)
                defens_role = defens[0].role
                db_role = await DbRoleManager().get_async_db_role(defens_role.role_id)
                builds = Builds(db_model=db_role)
                await builds.async_read()
                personaltech = PersonalTech(db_model=db_role)
                await personaltech.async_read()
                proxy = [builds, personaltech]
                if defens_role.guild_id:
                    db_guild = await DbGuildManager().get_async_db_guild(defens_role.guild_id)
                    guild = Guild(db_model=db_guild)
                    await guild.async_read()
                    proxy.append(guild)
                defens_role.add_blueprints(proxy)
                defens_role.refresh_role_buff()
                defens_battle_effect_list = await defens_role.get_async_buff_effect()
                for card in def_cards:
                    character = card.character
                    team_buff_list = character.get_team_buff_effect()
                    defens_battle_effect_list.extend(team_buff_list)

                battle_type = self.battle_type
                if battle_type == AIMTYPE.TEMPOSE:
                    battle_type = AIMTYPE.TEMPOSEDEFEN
                else:
                    battle_type = AIMTYPE.GUARDBLOCK

                for card in def_cards:
                    character_buff_list = card.character.get_character_buff_effect()
                    battle_effects = defens_battle_effect_list.copy() + character_buff_list
                    await self.async_add_card_buff(card, battle_effects, battle_type)
                    card.damage_buff = self.get_battle_damage_buff(battle_effects, battle_type)
                    card.resist_damage_buff = self.get_resist_damage_buff(battle_effects, battle_type)
                    card.exp_buff = self.get_exp_buff(battle_effects)

                battle.team_copy()
                battle.atk_battle_effects = self.get_buff_record(atks_battle_effect_list)
                battle.def_battle_effects = self.get_buff_record(defens_battle_effect_list)
                arr.append(battle)
        return arr

    async def update(self):
        if not self.is_init:
            return self.init()
        btlog = {}
        logs = []
        ca_time = 0
        # 检查是否还能继续战斗
        rt = self.check_over({})
        if rt != 2:
            await self.pvp.add_team_record_log(status="end", win=rt,
                                               atkteam=self.pvp.atk_leader_cards, is_atk_leader=0,
                                               defteam=self.pvp.def_leader_cards, is_def_leader=0,
                                               atk_fatigue_record=self.atk_fatigue_record,
                                               def_fatigue_record=self.def_fatigue_record)
            return
        battles = await self.person_person_battle()
        for battle in battles:
            result = battle.start()
            rt = 0 if result else 1
            atkfatigue, deffatigue, atkstratum, defstratum = self.pvp.record_card_fatigue_vule(
                battle.team_backup(), rt, self.atk_fatigue_record, self.def_fatigue_record
            )
            # 每一方记录击杀值
            team = battle.team_backup()
            self.pvp.update_team_kill(team[1], defstratum)
            self.pvp.update_team_kill(team[-1], atkstratum)

            log = battle.caclute_team_hp(self.pvp)
            logs.append(log)
            ca_time = max(ca_time, battle.battlelog.ca_time)
        self.pvp.curround += 1
        atkfatigue, deffatigue = self.pvp.record_current_fatigue_vule()
        btlog["logs"] = logs
        btlog["atk_fatigue"] = atkfatigue
        btlog["def_fatigue"] = deffatigue
        # 检查是否还能继续战斗
        rt = self.check_over(btlog)
        if rt != 2:
            await self.pvp.add_team_record_log(status="end", win=rt,
                                               atkteam=self.pvp.atk_leader_cards, is_atk_leader=0,
                                               defteam=self.pvp.def_leader_cards, is_def_leader=0,
                                               atk_fatigue_record=self.atk_fatigue_record,
                                               def_fatigue_record=self.def_fatigue_record)
            return
        return {"win": 2, "log": btlog, "round": self.pvp.curround, "ca_time": ca_time}


class GAPVPBattle:
    def __init__(self, atkteams, defenseteams, battle_type):
        self.battle_id = ""
        self.section = None  # 处于第几个阶段
        self.originalatk = atkteams  # 原始数据
        self.originaldefen = defenseteams
        self.atkbackup = copy.copy(atkteams)
        self.defenbackup = copy.copy(defenseteams)  # 大回合计算队列
        self.atkteams = []  # 小回合计算每次战斗的攻击队列
        self.defenseteams = []  # 小回合计算每次战斗的防守队列
        self.curround = 0
        self.curstage = 1
        self.first_section_win = 0
        self.init_kill()  # 初始化击杀数
        self.atk_max_fatigue, self.def_max_fatigue = self.record_current_fatigue_vule()
        self.team_record_log = {}
        self.team_record_log["log"] = []
        self.team_record_log["battle_id"] = ""
        self.battle_type = battle_type
        self.role_to_character_leader = {}  # 存储所有的pvp成员leader信息
        self.init_role_leader()  # 初始化所有角色的形象代理id

    def init_role_leader(self):
        for chas in self.originalatk:
            for cha in chas:
                if cha.is_leader:
                    rt = {}
                    rt["loco"] = cha.loco
                    rt["nickname"] = cha.nickname
                    rt["instance_id"] = cha.instance_id
                    rt["role_id"] = cha.role_id
                    self.role_to_character_leader[cha.role_id] = rt
        for chas in self.originaldefen:
            for cha in chas:
                if cha.is_leader:
                    rt = {}
                    rt["loco"] = cha.loco
                    rt["nickname"] = cha.nickname
                    rt["instance_id"] = cha.instance_id
                    rt["role_id"] = cha.role_id
                    self.role_to_character_leader[cha.role_id] = rt

    def get_role_leader(self, role_id):
        return self.role_to_character_leader[role_id]

    def init_kill(self):
        self.kill = KillInfos()  # 击杀数量（元气值）
        self.kill.add_team(self.originalatk)
        self.kill.add_team(self.originaldefen)

    def set_battle_id(self, battle_id):
        self.battle_id = battle_id

    def get_team_record_log(self):
        return self.team_record_log

    async def add_team_record_log(self, status, win,
                                  atkteam, is_atk_leader,
                                  defteam, is_def_leader,
                                  atk_buff_list=None, def_buff_list=None,
                                  atk_fatigue_record=None, def_fatigue_record=None):

        team_log = {}

        arr = []
        atk_hp = 0
        for atk in atkteam:
            cha = atk.character
            if cha:
                rt = cha.to_battle_info()
                rt["hp"] = int(atk.hp)
                atk_hp += atk.hp
                rt["role_id"] = atk.role_id
                rt["side"] = 1
                rt["nickname"] = atk.nickname
                rt["guild_name"] = atk.guild_name
                skills = []
                for skill in atk.skills:
                    skills.append({
                        "config_id": skill.config_id,
                        "level": skill.level,
                        "maxlevel": skill.maxlevel
                    })
                rt["skills"] = skills
                arr.append(rt)
        team_log["atk"] = arr
        team_log["atk_hp"] = atk_hp
        team_log["is_atk_leader"] = is_atk_leader
        team_log["atk_buff_list"] = atk_buff_list if atk_buff_list else {}

        arr = []
        def_hp = 0
        for defend in defteam:
            cha = defend.character
            if cha:
                rt = cha.to_battle_info()
                rt["hp"] = int(defend.hp)
                def_hp += defend.hp
                rt["role_id"] = defend.role_id
                rt["side"] = -1
                rt["nickname"] = defend.nickname
                rt["guild_name"] = defend.guild_name
                skills = []
                for skill in defend.skills:
                    skills.append({
                        "config_id": skill.config_id,
                        "level": skill.level,
                        "maxlevel": skill.maxlevel
                    })
                rt["skills"] = skills
                arr.append(rt)
        team_log["defend"] = arr
        team_log["def_hp"] = def_hp
        team_log["is_def_leader"] = is_def_leader
        team_log["def_buff_list"] = def_buff_list if def_buff_list else {}

        atk_fa, def_fa = self.record_current_fatigue_vule()  # 记录全部人员的总元气
        team_log["atk_fatigue"] = atk_fa
        team_log["def_fatigue"] = def_fa

        team_log["index"] = len(self.team_record_log["log"])
        team_log["status"] = status
        team_log["win"] = win

        if atk_fatigue_record:
            # print("atk_fatigue_record", atk_fatigue_record)
            for role_id, records in atk_fatigue_record.items():
                sum_total_fatigue = 0
                sum_lost_fatigue = 0
                sum_end_fatigue = 0
                role_win = 0
                role_lost = 0
                for config_id, record in records['detail'].items():
                    sum_total_fatigue += record['total_fatigue']
                    sum_lost_fatigue += record['lost_fatigue']
                    sum_end_fatigue += record['end_fatigue']
                    role_win = record['wins']
                    role_lost = record['lost']

                db_role = await DbRoleManager().get_async_db_role(role_id)
                role = Role(db_model=db_role)
                await role.async_read()
                records['role_id'] = role_id
                records['role_name'] = role.nickname
                records['role_avatar'] = role.avatar
                records['role_icon_frame'] = role.icon_frame
                records['total_fatigue'] = sum_total_fatigue
                records['lost_fatigue'] = sum_lost_fatigue
                records['end_fatigue'] = sum_end_fatigue
                records['wins'] = role_win
                records['lost'] = role_lost
            team_log["atk_fatigue_record"] = atk_fatigue_record

        if def_fatigue_record:
            # print("def_fatigue_record", def_fatigue_record)
            for role_id, records in def_fatigue_record.items():
                sum_total_fatigue = 0
                sum_lost_fatigue = 0
                sum_end_fatigue = 0
                role_win = 0
                role_lost = 0
                for config_id, record in records['detail'].items():
                    sum_total_fatigue += record['total_fatigue']
                    sum_lost_fatigue += record['lost_fatigue']
                    sum_end_fatigue += record['end_fatigue']
                    role_win = record['wins']
                    role_lost = record['lost']

                db_role = await DbRoleManager().get_async_db_role(role_id)
                role = Role(db_model=db_role)
                await role.async_read()
                records['role_id'] = role_id
                records['role_name'] = role.nickname
                records['role_avatar'] = role.avatar
                records['role_icon_frame'] = role.icon_frame
                records['total_fatigue'] = sum_total_fatigue
                records['lost_fatigue'] = sum_lost_fatigue
                records['end_fatigue'] = sum_end_fatigue
                records['wins'] = role_win
                records['lost'] = role_lost
            team_log["def_fatigue_record"] = def_fatigue_record

        self.team_record_log["log"].append(team_log)

    def get_pvp_character(self, instance_id, side):
        if side == 1:
            for team in self.atkbackup:
                for atk in team:
                    print("atk:get_pvp_character::", atk.instance_id, instance_id, side)
                    if atk.instance_id == instance_id:
                        return atk

        if side == -1:
            for team in self.defenbackup:
                for defen in team:
                    print("defen:get_pvp_character::", defen.instance_id, instance_id, side)
                    if defen.instance_id == instance_id:
                        return defen

        return None

    # 生成当前所有角色的疲劳值
    def record_current_fatigue_vule(self):
        atk_current_fatigue = 0
        for atks in self.atkbackup:
            for atk in atks:
                print("record_current_fatigue_vule atk:", atk.is_leader, atk.fatigue(), atk.instance_id)
                fatigue = atk.fatigue()
                if fatigue > 0:  # 如果疲劳值小于0 则不加入
                    atk_current_fatigue += fatigue

        def_current_fatigue = 0
        for defens in self.defenbackup:
            for defen in defens:
                print("record_current_fatigue_vule def:", defen.is_leader, defen.fatigue(), defen.instance_id)
                fatigue = defen.fatigue()
                if fatigue > 0:  # 如果疲劳值小于0 则不加入
                    def_current_fatigue += fatigue

        return atk_current_fatigue, def_current_fatigue

    def record_current_hp(self, battle):
        team = battle.team_backup()
        atk_hp = 0
        for atk in team[1]:
            atk_hp += atk.hp

        def_hp = 0
        for defen in team[-1]:
            def_hp += defen.hp

        return int(atk_hp), int(def_hp)

    def average_fatigue(self, characters, fatigue, basevalue=0, win=0, average_fatigue_record=None):
        if not average_fatigue_record:
            average_fatigue_record = {}
        rids = []
        stratum = {1: 0, 2: 0, 3: 0, 4: 0, 5: 0}
        total = 0
        for chas in characters:
            for cha in chas:
                rids.append(cha.role_id)
                total += cha.fatigue()
        if total == 0:
            total = 1
        for chas in characters:
            for cha in chas:
                fat = cha.fatigue()
                total_fatigue = copy.copy(cha.fatigue())
                per = fat / total
                fs = fatigue * per
                fat -= fs
                fat -= basevalue
                stratum[cha.stratum] += (fs + basevalue)
                cha.set_fatigue(fat)

                if cha.role_id not in average_fatigue_record:
                    average_fatigue_record[cha.role_id] = {}
                if 'detail' not in average_fatigue_record[cha.role_id]:
                    average_fatigue_record[cha.role_id]['detail'] = {}

                if cha.config_id not in average_fatigue_record[cha.role_id]['detail']:
                    average_fatigue_record[cha.role_id]['detail'][cha.config_id] = {
                        "config_id": cha.config_id,
                        "stratum": cha.stratum,
                        "lv": cha.level,
                        "total_fatigue": int(total_fatigue),
                        "lost_fatigue": int(fs + basevalue),
                        "end_fatigue": int(fat),
                        "wins": 0,
                        "lost": 0}
                else:
                    average_fatigue_record[cha.role_id]['detail'][cha.config_id]["lost_fatigue"] += int(fs + basevalue)
                    average_fatigue_record[cha.role_id]['detail'][cha.config_id]["end_fatigue"] = int(fat)

                if win == 0:
                    average_fatigue_record[cha.role_id]['detail'][cha.config_id]['wins'] += 1
                else:
                    average_fatigue_record[cha.role_id]['detail'][cha.config_id]['lost'] += 1

        rids = list(set(rids))

        return stratum, rids, average_fatigue_record

    # 记录单个卡牌元气
    def record_card_fatigue(self, cardsteam, side, win, fatigue_record):

        total = 0
        ratio = self.caculate_one_side_battle(cardsteam, side)
        if self.battle_type in [AIMTYPE.GUILDBLOCK] and side == -1:
            ratio *= (0.75)  # 如果是驻守在主堡或地格里则减少25%的消耗
        cards = cardsteam[side]
        for te in cards:
            cha = self.get_pvp_character(te.id, side)
            print("record_card_fatigue:", te.id, cha, win)
            if cha:
                win_bool = True if win == 0 else False
                start_fatigue, ength, end_fatigue = cha.caculate_fatigueVule_and_change_character(te, 2, win_bool, ratio)
                total += cha.fatigue()

                print("fatigue_record", fatigue_record)
                if cha.role_id not in fatigue_record:
                    fatigue_record[cha.role_id] = {}
                if 'detail' not in fatigue_record[cha.role_id]:
                    fatigue_record[cha.role_id]['detail'] = {}

                if cha.config_id not in fatigue_record[cha.role_id]['detail']:
                    fatigue_record[cha.role_id]['detail'][cha.config_id] = {
                        "config_id": cha.config_id,
                        "stratum": cha.stratum,
                        "lv": cha.level,
                        "total_fatigue": int(start_fatigue),
                        "lost_fatigue": int(ength),
                        "end_fatigue": int(end_fatigue),
                        "wins": 0,
                        "lost": 0}
                else:
                    fatigue_record[cha.role_id]['detail'][cha.config_id]["lost_fatigue"] += int(ength)
                    fatigue_record[cha.role_id]['detail'][cha.config_id]["end_fatigue"] = int(end_fatigue)

                if win == 0:
                    fatigue_record[cha.role_id]['detail'][cha.config_id]['wins'] += 1
                else:
                    fatigue_record[cha.role_id]['detail'][cha.config_id]['lost'] += 1

        return total, fatigue_record

    # 直接记录卡牌元气消耗， 准备存入卡牌
    def record_card_fatigue_vule(self, cardsteam, win, atk_fatigue_record, def_fatigue_record):
        atktotal = 0
        deftotal = 0
        atkstratum = {1: 0, 2: 0, 3: 0, 4: 0, 5: 0}
        defstratum = {1: 0, 2: 0, 3: 0, 4: 0, 5: 0}
        ratio = self.caculate_one_side_battle(cardsteam, 1)
        for te in cardsteam[1]:
            atk = self.get_pvp_character(te.id, 1)
            if atk:
                start_fatigue, fatigue, end_fatigue = atk.caculate_fatigueVule_and_change_character(te, 2, win == 0, ratio)
                atkstratum[atk.stratum] += fatigue
                atktotal += atk.fatigue()

                if atk.role_id not in atk_fatigue_record:
                    atk_fatigue_record[atk.role_id] = {}
                if 'detail' not in atk_fatigue_record[atk.role_id]:
                    atk_fatigue_record[atk.role_id]['detail'] = {}

                if atk.config_id not in atk_fatigue_record[atk.role_id]['detail']:
                    atk_fatigue_record[atk.role_id]['detail'][atk.config_id] = {
                        "config_id": atk.config_id,
                        "stratum": atk.stratum,
                        "lv": atk.level,
                        "total_fatigue": int(start_fatigue),
                        "lost_fatigue": int(fatigue),
                        "end_fatigue": int(end_fatigue),
                        "wins": 0,
                        "lost": 0}
                else:
                    atk_fatigue_record[atk.role_id]['detail'][atk.config_id]["lost_fatigue"] += int(fatigue)
                    atk_fatigue_record[atk.role_id]['detail'][atk.config_id]["end_fatigue"] = int(end_fatigue)

                if win == 0:
                    atk_fatigue_record[atk.role_id]['detail'][atk.config_id]['wins'] += 1
                else:
                    atk_fatigue_record[atk.role_id]['detail'][atk.config_id]['lost'] += 1

        ratio = self.caculate_one_side_battle(cardsteam, -1)
        if self.battle_type in [AIMTYPE.GUILDBLOCK]:
            ratio *= (0.75)  # 如果是驻守在主堡或地格里则减少25%的消耗
        for te in cardsteam[-1]:
            defen = self.get_pvp_character(te.id, -1)
            if defen:
                start_fatigue, fatigue, end_fatigue = defen.caculate_fatigueVule_and_change_character(te, 2, win != 0, ratio)
                defstratum[defen.stratum] += fatigue
                deftotal += defen.fatigue()

                if defen.role_id not in def_fatigue_record:
                    def_fatigue_record[defen.role_id] = {}
                if 'detail' not in def_fatigue_record[defen.role_id]:
                    def_fatigue_record[defen.role_id]['detail'] = {}

                if defen.config_id not in def_fatigue_record[defen.role_id]['detail']:
                    def_fatigue_record[defen.role_id]['detail'][defen.config_id] = {
                        "config_id": defen.config_id,
                        "stratum": defen.stratum,
                        "lv": defen.level,
                        "total_fatigue": int(start_fatigue),
                        "lost_fatigue": int(fatigue),
                        "end_fatigue": int(end_fatigue),
                        "wins": 0,
                        "lost": 0}
                else:
                    def_fatigue_record[defen.role_id]['detail'][defen.config_id]["lost_fatigue"] += int(fatigue)
                    def_fatigue_record[defen.role_id]['detail'][defen.config_id]["end_fatigue"] = int(end_fatigue)

                if win != 0:
                    def_fatigue_record[defen.role_id]['detail'][defen.config_id]['wins'] += 1
                else:
                    def_fatigue_record[defen.role_id]['detail'][defen.config_id]['lost'] += 1

        return atktotal, deftotal, atkstratum, defstratum

    def caculate_one_side_battle(self, cardsteam, myside):  # 我的所在方
        # 通过攻守双方的血量损失值获得
        total_target_atk = 0
        total_my_atk = 0
        for te in cardsteam[myside]:
            total_my_atk += int(te.attack)
        for te in cardsteam[-myside]:
            total_target_atk += int(te.attack)
        # 计算攻守双方
        print("caculate_one_side_battle:", total_my_atk, total_target_atk)
        if total_target_atk >= total_my_atk:
            return 1
        else:
            return 7.3 - 7 * math.pow((10 / 9), -(total_my_atk / total_target_atk))

    # 计算卡牌元气消耗， 只是计算并不用于记录
    def caculate_team_fatigue_vule(self, cardsteam, win):
        atktotal = 0
        deftotal = 0
        atkstratum = {1: 0, 2: 0, 3: 0, 4: 0, 5: 0}
        defstratum = {1: 0, 2: 0, 3: 0, 4: 0, 5: 0}
        ratio = self.caculate_one_side_battle(cardsteam, 1)
        for te in cardsteam[1]:
            atk = self.get_pvp_character(te.id, 1)
            if atk:
                fatigue = atk.caculate_fatigueVule(te, 2, win == 0, ratio)
                atkstratum[atk.stratum] += fatigue
                atktotal += fatigue
        ratio = self.caculate_one_side_battle(cardsteam, -1)
        if self.battle_type in [AIMTYPE.GUILDBLOCK]:
            ratio *= (0.75)  # 如果是驻守在主堡或地格里则减少25%的消耗
        for te in cardsteam[-1]:
            defen = self.get_pvp_character(te.id, -1)
            if defen:
                fatigue = defen.caculate_fatigueVule(te, 2, win != 0, ratio)
                print("caculate_team_fatigue_vule:", fatigue)
                defstratum[defen.stratum] += fatigue
                deftotal += fatigue

        return atktotal, deftotal, atkstratum, defstratum

    def total_fatigue_vule(self):
        atktotal = 0
        deftotal = 0
        for team in self.atkbackup:
            for atk in team:
                atktotal += atk.fatigue()
        for team in self.defenbackup:
            for defen in team:
                deftotal += defen.fatigue()

        return atktotal, deftotal

    def is_over(self):
        if len(self.atkbackup) == 0:  # 防守方胜
            return 1
        if len(self.defenbackup) == 0:  # 攻击方胜
            return 0
        return 2

    def set_curstage(self, index):
        self.curstage = index
        self.markstage = index

    async def start(self):
        self.section = FirstSection(self, self.battle_type)
        await self.section.handle(self.atkbackup, self.defenbackup)

    def check_teams(self):  # 检查所有角色是不是精力值为0
        # 统计所有的角色
        for atk in self.atkbackup:
            for cha in atk:
                if cha.is_fatigue():
                    if cha.is_leader:
                        self.atkbackup.remove(atk)
                        continue
                    else:
                        atk.remove(cha)

        for defen in self.defenbackup:
            for cha in defen:
                if cha.is_fatigue():
                    if cha.is_leader:
                        self.defenbackup.remove(defen)
                        continue
                    else:
                        defen.remove(cha)

    def reset_teams(self, side):
        if side == 1:
            self.defenseteams = copy.copy(self.defenbackup)
        elif side == -1:
            self.atkteams = copy.copy(self.atkbackup)
        return self.is_over()

    async def ga_round_one(self):
        return await self.section.update()

    def pvp_teams_log(self):
        # 根据实际需求返回队列成员长度
        cards = {}
        atks = self.atkbackup

        arr = []
        for team in atks:
            rt = None
            totalhp = 0
            for atk in team:
                if atk == 0 or atk == '0':
                    continue
                totalhp += atk.propertys.hp
                if atk.is_leader:
                    rt = atk.to_battle_info()
                    rt["is_leader"] = atk.loco
                    rt["role_id"] = atk.role_id
                    rt["owner_name"] = atk.nickname
                    rt["side"] = 1
            if rt:
                rt["maxhp"] = totalhp
                arr.append(rt)
        cards["atk"] = arr

        defens = self.defenbackup
        arr = []
        for team in defens:
            rt = None
            totalhp = 0
            for defen in team:
                if defen == 0 or defen == '0':
                    continue
                totalhp += defen.propertys.hp
                if defen.is_leader:
                    rt = defen.to_battle_info()
                    rt["is_leader"] = defen.loco
                    rt["role_id"] = defen.role_id
                    rt["owner_name"] = defen.nickname
                    rt["side"] = -1
            if rt:
                rt["maxhp"] = totalhp
                arr.append(rt)
        cards["def"] = arr
        atk_fa, def_fa = self.record_current_fatigue_vule()
        cards["fatigue"] = {"atk_fatigue": atk_fa, "def_fatigue": def_fa, "atk_max_fatigue": self.atk_max_fatigue,
                            "def_max_fatigue": self.def_max_fatigue}
        return {"round": self.curround, "team": cards, "stage": self.curstage, "wincamp": self.first_section_win}

    def change_section(self, section):
        self.section = section

    def update_team_kill(self, team, stratum):
        role_ids = []
        for cha in team:
            if cha.role_id not in role_ids:
                role_ids.append(cha.role_id)
        role_ids = list(set(role_ids))
        self.kill.update_kill(role_ids, stratum)

    def update_origin_character(self, atks, defens):
        for cha in atks:
            rtcha = self.get_pvp_character(cha.instance_id, 1)
            if rtcha:
                cha.set_fatigue(rtcha.fatigue())
            else:
                cha.set_fatigue(0)

        for cha in defens:
            rtcha = self.get_pvp_character(cha.instance_id, -1)
            if rtcha:
                cha.set_fatigue(rtcha.fatigue())
            else:
                cha.set_fatigue(0)
