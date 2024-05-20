from skills.script import Script
import random

class skill_50005(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_target_default()

        # 产生伤害
        skldata = self.create_skl_data()
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            real_hurt = en.hurt(self,hurt)
            maxdander = 100
            en.update_maxdander(maxdander)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        self.master.update_maxdander(100)
        skldata.set_eng(self.master)

         # 普攻创建技能日志
        self.create_skill_log(skldata,1)

        level4 = self.get_my_skill_level("500054")#查找500054技能的等级
        rd = random.randint(1,100)
        rates=[0,25,35,45,55,75]
        if level4 > 0 and rd <= rates[level4]:
            addhurts = [0,0.05,0.1,0.15,0.2,0.25]
            for en in enemys:
            # 给敌人添加中毒buff
                buf = self.create_buff(self.master, "buf_hp", {
                    "buff_id":"13002",
                    "value":self.master.attack*addhurts[level4],
                    "count":2,
                })

                rt = buf.is_effect(self.master, en)
                if rt:
                    self.skill_buff(en, buf)
                
    def get_skill_type(self):
        return 0