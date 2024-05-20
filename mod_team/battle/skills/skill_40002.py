from skills.script import Script
import random

class skill_40002(Script):
    def execute(self):
        # 筛选目标
        enemys = self.enemys_target_default()

        # 产生伤害
        skldata = self.create_skl_data()
        for en in enemys:
            hurt, htp = self.normal_hurt(en)
            real_hurt = en.hurt(self,hurt)
            self.master.update_maxdander(100)
            maxdander = 100
            en.update_maxdander(maxdander)
            skldata.set_eng(self.master)
            skldata.set_eng(en)
            skldata.add_des(en)
            skldata.add_hurt(real_hurt)
            skldata.add_htp(htp)

        # 普攻创建技能日志
        self.create_skill_log(skldata, 1)

        
        skill4level = self.get_my_skill_level("400024")
        pro=[0,10,15,20,25,30]
        for en in enemys:
            rd=random.randint(1,100)
            if rd <pro[skill4level]:
               # 随机给一个敌人添加昏迷buff
                buf = self.create_buff(self.master, "buf_sleep",{
                    "buff_id":"12006",
                    "count":1,
                })
                rt = buf.is_effect(self.master, en)
                if rt:
                    self.skill_buff(en, buf) 
        
    def get_skill_type(self):
        return 0
