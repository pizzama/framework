from game.core.mods.mod_team.battle.card import Card
from game.core.mods.mod_team.battle.battle_log import BattleLogManager
import copy


class Battle:
    def __init__(self, maxround=30, battleplace=0):
        self.battlelog = BattleLogManager()
        self.team = {1: [], -1: []}
        self.round = 0  # 回合数
        self.maxround = maxround  # 集结战斗回合数会增加到100
        self.totateamhp = {1: 0, -1: 0}
        self.battleplace = battleplace  # 战斗地点。判断是否要扣精力值 0 不扣

    def create_side_cards(self, team, side):
        self.team[side] = []
        self.totateamhp[side] = 0
        for index, te in enumerate(team):
            if te:
                # print("create_side_cards", te.serialize())
                card = Card(side, index, te)
                self.team[side].append(card)
                self.totateamhp[side] += card.maxhp
        return self.team[side]

    def set_side_cards(self, team, side):
        self.team[side] = team
        self.totateamhp[side] = 0
        for card in team:
            self.totateamhp[side] += card.maxhp

    def reset(self):
        self.battlelog.clean()
        self.round = 0  # 回合数

    def reset_side_property(self, side):
        teams = self.team[side]
        self.totateamhp[side] = 0
        for tm in teams:
            tm.reset_property()
            self.totateamhp[side] += tm.hp

    def accumulate_hp(self):
        self.leftteamhp = {1: 0, -1: 0}
        for cha in self.team[1]:
            self.leftteamhp[1] += cha.hp

        for cha in self.team[-1]:
            self.leftteamhp[-1] += cha.hp

    def round_single_go(self):
        # 记录队列信息
        self.create_team_log()
        self.check_death(self.team[1])  # 检查死亡
        self.check_death(self.team[-1])  # 检查死亡

        # 开始下一回合
        if len(self.team[-1]) == 0:
            # 攻击方胜利
            self.battlelog.result_record(0)
            return 0
        if len(self.team[1]) == 0:
            # 防守方胜利
            self.battlelog.result_record(1)
            return 1
        self.round += 1
        if self.round > self.maxround:
            # 超过最大回合
            self.accumulate_hp()  # 计算剩下的血量总和
            return self.deal_finish()
        # 增加log日志
        self.battlelog.round_record(self.round)
        self.battlelog.master_record(self.team[1] + self.team[-1])

        self.round_single(self.team[1] + self.team[-1])
        # 战斗后检查生死,为了合并数据
        self.check_death(self.team[1])  # 检查死亡
        self.check_death(self.team[-1])  # 检查死亡
        if len(self.team[-1]) == 0:
            # 攻击方胜利
            self.battlelog.result_record(0)
            return 0
        if len(self.team[1]) == 0:
            # 防守方胜利
            self.battlelog.result_record(1)
            return 1
        return 2

    def round_single(self, left):  # 单次回合计算
        left.sort(key=lambda x: x.speed, reverse=True)
        # 通过出手速度筛出攻击对象
        if len(left) > 0:
            ca = left.pop(0)
            # 检查被攻击方是否有死亡
            arr = []
            for cha in self.team[-ca.side]:
                if not cha.is_dead():
                    arr.append(cha)
            if len(arr) == 0:
                return self.round_single([])
            # 增加log日志
            ca.start_battle(self.team[ca.side], arr, self.battlelog)
            self.check_death(left)
        else:
            return 2

        return self.round_single(left)

    # 每回合触发函数和额外的参数
    def start(self, round_action=None, extdata1=None, extdata2=None):  #
        self.round_action = round_action
        self.extdata1 = extdata1
        self.extdata2 = extdata2
        self.create_team_log()
        # 记录战斗日志
        self.round += 1
        self.battlelog.round_record(self.round)
        self.battlelog.master_record(self.team[1] + self.team[-1])
        self.skill_before_battle()
        self.record_team_hp()
        rt = self.round_one(self.team[1] + self.team[-1])
        return self.battlelog.result_record(rt)

    # 记录上一次的血量用于疲劳值计算
    def record_team_hp(self):
        for card in self.team[1]:
            card.record_hp()
        for card in self.team[-1]:
            card.record_hp()

    def check_win(self):
        self.check_death(self.team[1])  # 检查死亡
        self.check_death(self.team[-1])  # 检查死亡
        if len(self.team[-1]) == 0:
            # 攻击方胜利
            return 0
        if len(self.team[1]) == 0:
            # 防守方胜利
            return 1
        return 2

    def skill_before_battle(self):
        # 混合攻守双方，按照速度排序
        all = self.team[1] + self.team[-1]
        all.sort(key=lambda x: x.speed)  # 升序
        for ca in all:
            side = ca.side
            if side == 1:
                ca.skill_before_battle(self.team[1], self.team[-1], self.battlelog)
            elif side == -1:
                ca.skill_before_battle(self.team[-1], self.team[1], self.battlelog)

    def round_one(self, left):  # left 剩余出手人次
        if self.round_action:
            rt = self.round_action(self, self.extdata1, self.extdata2)
            if rt is False:
                return 1
        if len(left) == 0:
            self.check_death(self.team[1])  # 检查死亡
            self.check_death(self.team[-1])  # 检查死亡

            # 开始下一回合
            if len(self.team[-1]) == 0:
                # 攻击方胜利
                return 0
            if len(self.team[1]) == 0:
                # 防守方胜利
                return 1
            self.round += 1
            if self.round > self.maxround:
                # 超过最大回合
                self.accumulate_hp()  # 计算剩下的血量总和
                self.deal_finish()
                return 2
            print("start round %s" % self.round)
            # 增加log日志
            self.battlelog.round_record(self.round)
            self.battlelog.master_record(self.team[1] + self.team[-1])
            self.skill_before_battle()

            return self.round_one(self.team[1] + self.team[-1])
        else:
            left.sort(key=lambda x: x.speed, reverse=True)
            # 通过出手速度筛出攻击对象
            if len(left) > 0:
                ca = left.pop(0)
                # 检查被攻击方是否有死亡
                arr = []
                for cha in self.team[-ca.side]:
                    if not cha.is_dead():
                        arr.append(cha)
                if len(arr) == 0:
                    return self.round_one([])

                ca.start_battle(self.team[ca.side], arr, self.battlelog)

                self.check_death(left)

            return self.round_one(left)

    def get_round(self):
        return self.round

    def get_atkteam_num(self):  # 获得减少的血量
        return len(self.team[1])

    def deal_finish(self):
        atotal = round(self.totateamhp[1])
        aleft = round(self.leftteamhp[1])
        dtotal = round(self.totateamhp[-1])
        dleft = round(self.leftteamhp[-1])

        atemp = atotal - aleft
        dtemp = dtotal - dleft
        aper = atemp / atotal
        dper = dtemp / dtotal

        if aper > dper:  # 攻击方清零， 防守方按百分比掉血
            per = dper / aper
            for ca in self.team[1]:
                ca.hp = 0
            for ca in self.team[-1]:
                ca.hp -= round(ca.hp * per)
            return 1
        elif aper < dper:  # 防守方清零， 攻击方安百分比掉血
            per = aper / dper
            for ca in self.team[-1]:
                ca.hp = 0
            for ca in self.team[1]:
                ca.hp -= round(ca.hp * per)
            return 0
        else:
            return 1

    def get_side(self, side):
        return self.team[side]

    def check_death(self, arr):  # 检查死亡
        for ta in arr[:]:
            if ta.is_dead():
                arr.remove(ta)

    def check_buff(self):
        arr = self.team[1]
        for he in arr:
            he.check_buff()

    def to_log(self):
        return self.battlelog.to_log()

    def to_team_log(self):
        return self.battlelog.to_team_log()

    def to_slog(self):
        return self.battlelog.to_slog()

    def create_team_log(self):
        self.battlelog.make_team_log(self.team[1], self.team[-1])

    def caclute_team_hp(self, pvp):
        return self.battlelog.caclute_team_hp(self, pvp)

    def caclute_pve_team_hp(self, gabattle):
        return self.battlelog.caclute_pve_team_hp(self, gabattle)

    def copy_win_side(self):
        atkteam = self.team[1]
        if len(atkteam) > 0:
            return copy.copy(atkteam), 1
        defteam = self.team[-1]
        if len(defteam) > 0:
            return copy.copy(defteam), -1

    def team_copy(self):
        self.teambackup = {1: [], -1: []}
        for atk in self.team[1]:
            self.teambackup[1].append(atk)
        for defen in self.team[-1]:
            self.teambackup[-1].append(defen)
        return self.teambackup

    def team_backup(self):
        return self.teambackup

