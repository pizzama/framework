from skills.script import Script
import random


class skill_4000251(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        level = self.skill.level
        skldata = self.create_skl_data()
        damage = [0,2.5,2.5,2.5,2.5,2.5]
        treat = [0,0.5,0.5,0.5,0.5,0.5]
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

        # 为自己恢复血量
        hp = -self.master.attack*treat[level]
        self.master.hurt(self,hp)
        skldata.add_des(self.master)
        skldata.add_hurt(hp)
        

        # 创建技能日志
        self.create_skill_log(skldata)
        
        rd=random.randint(1,100)
        skill4level = self.get_my_skill_level("400024")
        pro=[0,10,15,20,25,30]
        if rd <pro[skill4level]:
            for en in enemys:
               # 随机给一个敌人添加昏迷buff
                buf = self.create_buff(self.master, "buf_sleep",{
                "buff_id":"12006",
                "count":1,
            })
            rt = buf.is_effect(self.master, en)
            if rt:
                self.skill_buff(en, buf) 