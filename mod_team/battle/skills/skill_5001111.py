from skills.script import Script
import random


class skill_5001111(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,1.6,2.0,2.4,2.8,3.2]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = hurt*damage[level]*(1-0.25*min((num-1),2))
            real_hurt = en.hurt(self,hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        
        # 创建技能日志
        self.create_skill_log(skldata)

        # 概率给敌人附加昏睡效果
        pro=[0,5,10,15,20,25]
        for en in enemys:
            rd = random.randint(1,100)
            if rd < pro[level]:
                buf = self.create_buff(self.master, "buf_sleep", {
                    "buff_id": "12005",
                })

                rt = buf.is_effect(self.master, en)
                if rt:
                    self.skill_buff(en, buf)


