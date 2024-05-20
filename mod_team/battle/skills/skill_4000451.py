from skills.script import Script
import random


class skill_4000451(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,0.9,0.9,0.9,0.9,0.9]
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = damage[level]*hurt
            real_hurt = en.hurt(self,hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

        # 创建技能日志
        self.create_skill_log(skldata)

        # 概率给敌人附加昏睡效果
        pro=[0,35,35,35,35,35]
        for en in enemys:
            rd = random.randint(1,100)
            if rd < pro[level]:
                buf = self.create_buff(self.master, "buf_sleep", {
                    "buff_id": "12005",
                })

                rt = buf.is_effect(self.master, en)
                if rt:
                    self.skill_buff(en, buf)

                # 创建buff日志
                self.create_buff_log(buf)

        # 创建技能日志
        self.create_skill_log(skldata)
