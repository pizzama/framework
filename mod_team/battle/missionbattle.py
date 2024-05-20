from game.exceptions import GameException
from game.core.mods.mod_team.battle.battle import Battle


class MissionBattle:
    def __init__(self, atkteams, defenseteams):
        self.atkteams = atkteams
        self.defenseteams = defenseteams

    def get_atk_teams_hp(self, role_buff_effect):
        hp = 0
        for team in self.atkteams:
            team_buff_effect = role_buff_effect.copy()
            for ta in team:
                if ta:
                    team_buff_effect += ta.get_team_buff_effect()
            for ta in team:
                if ta:
                    character_buff_effect = ta.get_character_buff_effect()
                    team_buffs = team_buff_effect.copy() + character_buff_effect
                    country = ta.get_country()
                    hp_buff = [0, 0, 0]
                    for buff in team_buffs:
                        if buff[2] == 12 and buff[3] in (0, country):
                            hp_buff[1] += buff[4]
                        elif buff[2] == 46 and buff[3] in (0, country):
                            hp_buff[0] += buff[4]
                        elif buff[2] == 47 and buff[3] in (0, country):
                            hp_buff[2] += buff[4]
                        elif buff[2] == 50 and buff[3] in (0, country):
                            hp_buff[1] += buff[4]
                    _hp = ta.get_hp()
                    _hp += hp_buff[2]
                    _hp *= (100 + hp_buff[1]) / 100
                    _hp += hp_buff[0]
                    hp += _hp
        return hp

    @staticmethod
    def get_hp_buff(battle_effect_list, country):
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
        return hp_buff

    @staticmethod
    def get_attack_buff(battle_effect_list, country):
        attack_buff = [0, 0, 0]
        for buff in battle_effect_list:
            if buff[2] == 48 and buff[3] in (0, country):
                attack_buff[2] += buff[4]
            elif buff[2] == 51 and buff[3] in (0, country):
                attack_buff[1] += buff[4]
        return attack_buff

    @staticmethod
    def get_defen_buff(battle_effect_list, country):
        defen_buff = [0, 0, 0]
        for buff in battle_effect_list:
            if buff[2] == 49 and buff[3] in (0, country):
                defen_buff[2] += buff[4]
            elif buff[2] == 52 and buff[3] in (0, country):
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
    def get_battle_damage_buff(battle_effect_list):
        damage_buff = {1: [0, 0], 2: [0, 0], 3: [0, 0], 4: [0, 0]}
        for buff in battle_effect_list:
            if buff[2] == 32:
                print("get_battle_damage_buff32:::", buff)
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
            if buff[2] == 66:
                print("get_battle_damage_buff66:::", buff)
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
                print("get_battle_damage_buff67:::", buff)
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
    def get_exp_buff(battle_effect_list):
        exp_buff = [0, 0]
        for buff in battle_effect_list:
            if buff[2] == 33:
                exp_buff[1] += buff[4]
        return exp_buff

    def add_card_buff(self, card, battle_effect_list):
        country = card.country
        hp_buff = self.get_hp_buff(battle_effect_list, country)
        card.hp += hp_buff[2]
        card.hp *= (100 + hp_buff[1]) / 100
        card.hp += hp_buff[0]
        card.maxhp = card.hp
        attack_buff = self.get_attack_buff(battle_effect_list, country)
        card.attack += attack_buff[2]
        card.attack *= (100 + attack_buff[1]) / 100
        card.attack += attack_buff[0]
        defen_buff = self.get_defen_buff(battle_effect_list, country)
        card.defen += defen_buff[2]
        card.defen *= (100 + defen_buff[1]) / 100
        card.defen += defen_buff[0]
        speed_buff = self.get_speed_buff(battle_effect_list, country)
        card.speed *= (100 + speed_buff[1]) / 100
        effectshit_buff = self.get_effectshit_buff(battle_effect_list, country)
        card.effectshit += effectshit_buff[0]
        effectsdef_buff = self.get_effectsdef_buff(battle_effect_list, country)
        card.effectsdef += effectsdef_buff[0]

    def check_avaliabel_member(self, teams):
        count = 0
        for ta in teams:
            if ta:
                count += 1
        return count

    def start(self, mission, role_buff_effect, smulator=False):
        if mission:
            mission.init()
        # 战斗
        arr = []
        win = True
        atk = None
        defense = None
        self.totalround = 0
        count = self.check_avaliabel_member(self.atkteams)
        if count == 0:
            raise GameException(status=117)
        self.maxhp = self.get_atk_teams_hp(role_buff_effect)

        battle = Battle()
        # 总共多少波次怪物
        totalenemys = len(self.defenseteams)
        while True:
            if atk is None:
                if len(self.atkteams) == 0:
                    win = False
                    break
                atk = self.atkteams.pop(0)
                battle.create_side_cards(atk, 1)
                self.leftnum = battle.get_atkteam_num()

                # 遍历队列，获得character上的buff
                battle_effect_list = role_buff_effect
                for card in battle.team[1]:
                    character = card.character
                    character_buff = character.get_team_buff_effect()
                    battle_effect_list.extend(character_buff)

                for card in battle.team[1]:
                    character_buff_list = card.character.get_character_buff_effect()
                    battle_effects = battle_effect_list.copy() + character_buff_list
                    self.add_card_buff(card, battle_effects)
                    card.damage_buff = self.get_battle_damage_buff(battle_effects)
                    card.exp_buff = self.get_exp_buff(battle_effects)
                    card.copy_property() # 记录战斗中的数值

            if defense is None:
                if len(self.defenseteams) == 0:
                    win = True
                    break
                defense = self.defenseteams.pop(0)
                battle.create_side_cards(defense, -1)

            def round_action(bt, ext1, ext2):
                mission = ext1
                missionbattle = ext2
                totalround = missionbattle.totalround
                totalround += bt.get_round()
                num = bt.get_atkteam_num()
                dead_num = self.leftnum - num
                left = len(missionbattle.atkteams) + 1
                win = bt.check_win()
                rt = mission.condition_check(dead_num, totalround, left, win)
                if len(rt) > 0:
                    bt.battlelog.lck_record("")
                    bt.battlelog.mission_log(rt)
                    bt.battlelog.rls_record("")
                return mission.checkiswin()

            iswin = battle.start(round_action, mission, self)
            if iswin:
                defense = None
                battle.reset_side_property(1)
            else:
                atk = None
                battle.reset_side_property(-1)
            rt = {}
            rt["team"] = battle.to_team_log()
            if smulator:
                rt["slog"] = battle.to_slog()
            rt["log"] = battle.to_log()
            rt["enemyindex"] = totalenemys - len(self.defenseteams)
            arr.append(rt)
            battle.reset()
        # 结算
        data = {}
        data["log"] = arr
        data["win"] = win
        return data

    def arena_start(self, role_buff_effect, smulator=False):
        # 战斗
        arr = []
        win = True
        atk = None
        defense = None
        self.totalround = 0
        count = self.check_avaliabel_member(self.atkteams)
        if count == 0:
            raise GameException(status=117)
        self.maxhp = self.get_atk_teams_hp(role_buff_effect)

        battle = Battle()
        # 总共多少波次怪物
        totalenemys = len(self.defenseteams)
        while True:
            if atk is None:
                if len(self.atkteams) == 0:
                    win = False
                    break
                atk = self.atkteams.pop(0)
                self.lefthp = self.get_atk_teams_hp(role_buff_effect)
                battle.create_side_cards(atk, 1)

                # 遍历队列，获得character上的buff
                battle_effect_list = role_buff_effect
                for card in battle.team[1]:
                    character = card.character
                    character_buff = character.get_team_buff_effect()
                    battle_effect_list.extend(character_buff)

                for card in battle.team[1]:
                    character_buff_list = card.character.get_character_buff_effect()
                    battle_effects = battle_effect_list.copy() + character_buff_list
                    self.add_card_buff(card, battle_effects)
                    card.damage_buff = self.get_battle_damage_buff(battle_effects)
                    card.exp_buff = self.get_exp_buff(battle_effects)

            if defense is None:
                if len(self.defenseteams) == 0:
                    win = True
                    break
                defense = self.defenseteams.pop(0)
                battle.create_side_cards(defense, -1)

            iswin = battle.start()
            if iswin:
                defense = None
                battle.reset_side_property(1)
            else:
                atk = None
                battle.reset_side_property(-1)
            rt = {}
            rt["team"] = battle.to_team_log()
            if smulator:
                rt["slog"] = battle.to_slog()
            rt["log"] = battle.to_log()
            rt["enemyindex"] = totalenemys - len(self.defenseteams)
            arr.append(rt)
            battle.reset()
        # 结算
        data = {}
        data["log"] = arr
        data["win"] = win
        return data