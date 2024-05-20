from skills.script import Script
import random


class skill_3000411(Script):
    def execute(self):
        # 筛选目标
        enemys = self.heros_all()

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,-0.45,-0.5,-0.55,-0.6,-0.65]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = damage[level]*self.master.attack
            real_hurt = en.hurt(self,hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

        # 创建技能日志
        self.create_skill_log(skldata)
        
        pro=[0,50,60,70,80,90]
        rd=random.randint(1,100)
        if rd < pro[level]:
            for en in enemys:
                buff_arr = en.has_buff_id(["13001","13002"])
                if len(buff_arr) > 0:
                    self.remove_buff(en, ["13001","13002"])