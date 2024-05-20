from skills.script import Script
import random

class skill_4001011(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,1.25,1.5,1.75,2.0,2.25]
        pro=[0,0.02,0.04,0.07,0.1,0.15]
        defens=[0,0.02,0.04,0.06,0.08,0.1]
        leiyin = self.master.get_extraparams("leiyin")
        skill4level = self.get_my_skill_level("400104")
        self.master.update_maxdander(0)
        skldata.set_eng(self.master)
        for en in enemys:
            # 增加特殊标记
            hurt, htp = self.normal_hurt(en)
            hurt = hurt*damage[level]*(1-0.25*min((num-1),2))+ en.defen*defens[skill4level]*leiyin
            real_hurt = en.hurt(self, hurt)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

        # 创建技能日志
        self.create_skill_log(skldata)
        
        rd = random.randint(1,100)
        for en in enemys:
            if rd < (pro[skill4level]*leiyin):
                buf = self.create_buff(self.master, "buf_sleep", {
                    "buff_id": "12001",
                    })

                rt = buf.is_effect(self.master, en)
                if rt:
                    self.skill_buff(en, buf)
        leiyin = 0
        self.master.set_extraparams("leiyin", leiyin)


        


