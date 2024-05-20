from skills.script import Script
import random

class skill_50002(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_all()
        num=len(enemys)

        # 产生伤害
        skldata = self.create_skl_data()
        addhurt=[0,1,0.6,0.4]
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            hurt = addhurt[num]*hurt
            real_hurt = en.hurt(self,hurt)
            maxdander = 50
            en.update_maxdander(maxdander)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)
        self.master.update_maxdander(100)
        skldata.set_eng(self.master)

        # 普攻创建技能日志
        self.create_skill_log(skldata, 1)

        
        skill4level = self.get_my_skill_level("500024")
        pro=[0,5,10,15,20,30]
        for en in enemys:
            rd=random.randint(1,100)
            if rd <pro[skill4level]:
               # 随机给一个敌人添加嘲讽buff
                buf = self.create_buff(self.master, "buf_taunt",{
                    "buff_id":"12003",
                    "count":1,
                })
                
                rt = buf.is_effect(self.master, en)
                if rt:
                    self.skill_buff(en, buf) 
        
    def get_skill_type(self):
        return 0
